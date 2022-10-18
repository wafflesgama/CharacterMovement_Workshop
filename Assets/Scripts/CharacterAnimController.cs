using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    public CharacterMovController movController;
    public PlayerInputHandler playerInputHandler;

    [Header("Model Rotation")]
    public float rotationSpeed = 5;

    [ReadOnly] public Vector3 playerDirection;
    [ReadOnly] public Quaternion playerRotation;

    private Animator animator;
    private UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInputHandler.input_interact.Onpressed.Subscribe(eventHandler, Kick);
    }
    void Start()
    {
        movController.OnJump.Subscribe(eventHandler, Jump);
        movController.OnLand.Subscribe(eventHandler, Land);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    // Update is called once per frame
    void Update()
    {

        var vel = movController.rb.velocity;
        vel.y = 0;


        var magnitude = vel.sqrMagnitude;
        animator.SetFloat("Speed", magnitude);

        vel.Normalize();

        if (magnitude > 0 && vel != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), rotationSpeed * Time.deltaTime);
            //transform.forward = Vector3.Lerp(transform.forward, vel, Time.deltaTime * rotationSpeed);
        }
        playerRotation = transform.rotation;
        playerDirection = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
        Debug.DrawRay(transform.position, playerDirection);
    }

    void Jump()
    {
        animator.SetTrigger("Jump");
    }

    void Land()
    {
        animator.SetTrigger("Land");
    }


    void Kick()
    {
        animator.SetTrigger("Kick");
    }

    public void Knock()
    {
        animator.SetTrigger("Knock");
    }

    public void Recover()
    {
        animator.SetTrigger("Recover");
    }

}
