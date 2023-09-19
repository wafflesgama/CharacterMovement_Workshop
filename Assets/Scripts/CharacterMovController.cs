using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovController : MonoBehaviour
{

    public PlayerInputHandler inputHandler;
    [ReadOnly] public Rigidbody rb;


    [Header("Float")]
    [Range(0,2)]
    public float floatHeight = 1.6f;
    public float springStrength = 60f;
    public float dampenStrength = 29f;

    [Header("Ground Check")]
    public float minGroundHeight = 2f;
    public float castDistance = 2f;
    public LayerMask raycastMask =1;
    [ReadOnly] public bool isGroundNear;
    [ReadOnly] public bool isGrounded;
    [ReadOnly] [SerializeField] private RaycastHit groundHit;

    [Header("Move")]
    public float maxAccel = 120f;
    public float maxDeccel = 200f;
    public float goalVelocity=10f;

    [Header("Jump")]
    public float jumpForce=9.5f;
    public int jumpMaxDurationFrames = 15;
    public float jumpDownForce = 111f;
    public UEvent OnJump = new UEvent();
    [ReadOnly] [SerializeField] private bool isJumping;
    [ReadOnly] [SerializeField] private int jumpDurationCounter;

    [Header("Land")]
    public int landingStartCheckFrames = 8;
    public UEvent OnLand = new UEvent();
    [ReadOnly] [SerializeField] private bool isLanding;
    [ReadOnly] [SerializeField] private int landStartCheckCounter;

    [Header("Airbourne")]
    public float airDeacFactor=0f;


    [Header("Stats")]
    public Vector3 vel;
    public Vector3 horizontalVel;
    public float horizontalVelMag;
    [ReadOnly] public bool isFrozen;

    private UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //In case the user inputs a float height 
        if (minGroundHeight < floatHeight)
            floatHeight = minGroundHeight;

        if (castDistance < floatHeight * 1.2f)
            castDistance = floatHeight * 1.2f;
    }



    void Start()
    {
        inputHandler.input_jump.Onpressed.Subscribe(eventHandler, Jump);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    private void FixedUpdate()
    {
        //CheckGround();

        //CheckStartLanding();
        //JumpDownForce();
        //CheckEndLanding();

        //Float();

        //Move();

        //AirbourneDecel();
        //JumpUpdate();
    }


    private void LateUpdate()
    {
        //Store the Rigidbody velocity at the end of the frame
        vel = rb.velocity;

        horizontalVel = vel;
        horizontalVel.y = 0f;

        horizontalVelMag = horizontalVel.magnitude;
    }


    private void CheckGround()
    {
        //Checks if raycast hits something
        isGroundNear = Physics.Raycast(transform.position, -transform.up, out groundHit, castDistance, raycastMask, QueryTriggerInteraction.Ignore);

        //If it hits something checks if distance is the minimum required
        isGrounded = isGroundNear ? groundHit.distance <= minGroundHeight : false;
    }


    public float  heightHeight=20f;
    private void Float()
    {
        //Initial validations
        if (!isGroundNear || isJumping) return;

        //Initial parameters
        Vector3 outsideVel = Vector3.zero;

        //If ground object has velocity store it
        if (groundHit.rigidbody != null)
            outsideVel = groundHit.rigidbody.velocity;


        float dirOwnVel = Vector3.Dot(-transform.up, rb.velocity);
        float dirOutsideVel = Vector3.Dot(-transform.up, outsideVel);
        float dirRelation = dirOwnVel - dirOutsideVel;

        float distance = groundHit.distance - floatHeight;

        //Calculate spring force necessary
        float spring = (distance * springStrength) - (dirRelation * dampenStrength);


        //Add float force to character
        rb.AddForce(-transform.up * rb.mass * spring);

        //If ground has rigidbodt apply down force to it
        if (outsideVel != Vector3.zero)
        {
            groundHit.rigidbody.AddForceAtPosition(-transform.up * -springStrength, groundHit.point);
        }

    }
    private void Move()
    {
        //Initial validations
        if (!isGrounded || isFrozen) return;

        //Get input direction and transform it to the camera orientation
        Vector2 move = inputHandler.input_move.value;
        Vector3 transformedMove = inputHandler.playerCamera.transform.TransformDirection(new Vector3(move.x, 0, move.y));
        transformedMove.y = 0;

        //Calculate necessary aceleration to achieve goal velocity
        Vector3 aceleration = (transformedMove * goalVelocity - rb.velocity) / Time.fixedDeltaTime;


        //Check if new direction is facing or against current velocity
        float dot = Vector3.Dot(transformedMove, rb.velocity);

        if (dot >= 0)
            aceleration = Vector3.ClampMagnitude(aceleration, maxAccel);  //If positive or zero apply acceleration clamp
        else
            aceleration = Vector3.ClampMagnitude(aceleration, maxDeccel);  //If negative apply decceleration clamp

        Vector3 force = rb.mass * aceleration;
        //Make sure Move Force does apply to Y axis
        force.y = 0;

        rb.AddForce(force);
    }





    private void Jump()
    {
        if (!isGroundNear || isJumping || isFrozen) return;


        //Init Jump variables
        isJumping = true;
        landStartCheckCounter = landingStartCheckFrames;
        jumpDurationCounter = jumpMaxDurationFrames;

        //Invoke event to outside listeners
        OnJump.TryInvoke();

        //Add the force
        rb.AddForce(Vector3.up * rb.mass * jumpForce, ForceMode.Impulse);
    }

    private void JumpDownForce()
    {
        if (isGroundNear || (inputHandler.input_jump.value > 0 && jumpDurationCounter > 0) || landStartCheckCounter > 0) return;

        rb.AddForce(Vector3.down * rb.mass * jumpDownForce);

    }

    private void CheckStartLanding()
    {
        if (!isJumping || landStartCheckCounter > 0) return;

        if (rb.velocity.y > 0) return;

        isLanding = true;
        isGrounded = false;
        isJumping = false;
    }
    private void CheckEndLanding()
    {
        if (!isLanding || !isGrounded) return;

        isLanding = false;
        OnLand.TryInvoke();
    }

    private void AirbourneDecel()
    {
        if (isGroundNear || rb.velocity == Vector3.zero) return;
        var force = -rb.velocity * rb.mass * airDeacFactor;
        force.y = 0f;
        rb.AddForce(force);
    }

    private void JumpUpdate()
    {

        if (jumpDurationCounter > 0)
            jumpDurationCounter--;

        if (landStartCheckCounter > 0)
            landStartCheckCounter--;
    }

    public Vector3 getMoveDirection()
    {
        return rb.velocity;
    }

    public void FreezePlayer(bool unfreeze = false)
    {
        isFrozen = !unfreeze;
        rb.isKinematic = !unfreeze;
    }
}
