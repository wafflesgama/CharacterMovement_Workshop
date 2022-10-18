using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GoalController : MonoBehaviour
{
    public PlayerTriggerChecker checker;

    public VisualEffect goalVictory;

    public UEventHandler eventHandler = new UEventHandler();

    void Start()
    {
        checker.OnTriggered.Subscribe(eventHandler, GoalScored);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void GoalScored()
    {
        goalVictory.SendEvent("OnScored");
        GameManager.instance.ScoredGoal(gameObject.tag == "Goal 1");
    }
}
