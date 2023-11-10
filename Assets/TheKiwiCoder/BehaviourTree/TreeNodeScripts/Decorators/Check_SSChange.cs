using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_SSChange : DecoratorNode
{
    private SensorySource initialSS = null;
    protected override void OnStart()
    {
        initialSS = npcController.GetCurrentSS;
        blackboard.resetFlag = false;
    }

    protected override void OnStop() {
        blackboard.resetFlag = false;
        initialSS = null;
    }

    protected override State OnUpdate() {

        if (initialSS!=null&&!initialSS.Equals(blackboard.currentSensorySource)||blackboard.resetFlag)
        {
            Abort();
            return State.Failure;
        }

        //if NPC controller and blackboard de-synced
        if (initialSS != null && !initialSS.Equals(npcController.GetCurrentSS))
        {
            Abort();
            return State.Failure;
        }

        if (blackboard.currentSensorySource != null)
        {
            initialSS = blackboard.currentSensorySource;
            // Debug.Log($"Current SS: {blackboard.currentSensorySource.SmartObject.ToString()}");
        }

        return child.Update();
    }
}
