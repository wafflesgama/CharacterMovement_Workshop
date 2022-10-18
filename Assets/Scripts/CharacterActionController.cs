using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    public CharacterMovController movController;

    public PlayerTriggerChecker checker;

    public float kickForce = 10f;
    private UEventHandler eventHandler = new UEventHandler();

    void Start()
    {
        inputHandler.input_interact.Onpressed.Subscribe(eventHandler, Kick);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }


    void Kick()
    {
        if (!checker.hasObject) return;
        if (checker.objRb == null) return;

        var dir = movController.horizontalVel.normalized;
        checker.objRb.AddForce(dir * kickForce, ForceMode.Impulse);
    }
}
