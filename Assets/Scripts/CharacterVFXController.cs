using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterVFXController : MonoBehaviour
{
    public CharacterMovController mov;

    [Header("Dust Trail")]
    public VisualEffect dustVfx;
    public int dustRate;
    public float minSpeed;
    public AnimationCurve dustCurve;
    int dustRateVar, dustJumpVar, dustLandVar;
    bool jumpFlag = true;

    [Header("Knock")]
    public VisualEffect knockVfx;
    int aliveVar;

    [Header("Hit")]
    public VisualEffect hitVfx;
    int hitVar, hitEvent;

    UEventHandler eventHandler = new UEventHandler();


    private void Awake()
    {
        dustRateVar = Shader.PropertyToID("Rate");
        dustJumpVar = Shader.PropertyToID("OnJump");
        dustLandVar = Shader.PropertyToID("OnLand");
        aliveVar = Shader.PropertyToID("Alive");
        hitVar = Shader.PropertyToID("Pos");
        hitEvent = Shader.PropertyToID("OnHit");
    }
    void Start()
    {
        mov.OnJump.Subscribe(eventHandler, Jump);
        mov.OnLand.Subscribe(eventHandler, Land);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    private void Jump()
    {
        //if (!jumpFlag) return;

        dustVfx.SendEvent(dustJumpVar);
        //jumpFlag= false;
    }
    private void Land()
    {
        //jumpFlag= true;
        dustVfx.SendEvent(dustLandVar);
    }
    private void LateUpdate()
    {
        AnimateDust();
    }

    void AnimateDust()
    {
        //if (!mov.isSprinting) return;
        var show = mov.horizontalVelMag > minSpeed ? 1 : 0;
        var speedFraction = mov.horizontalVelMag / mov.goalVelocity;
        float dustAmount = dustCurve.Evaluate(speedFraction) * dustRate * show;
        dustAmount = mov.isGrounded ? dustAmount : 0;
        dustVfx.SetFloat(dustRateVar, dustAmount);
    }

    [ContextMenu("Knock")]
    public void Knock(Vector3 pos)
    {
        hitVfx.SetVector3(hitVar, pos);
        hitVfx.SendEvent(hitEvent);

        knockVfx.SetBool(aliveVar, true);
        knockVfx.Play();
    }

    [ContextMenu("Recover")]
    public void Recover()
    {
        knockVfx.SetBool(aliveVar, false);
        knockVfx.Stop();
    }
}
