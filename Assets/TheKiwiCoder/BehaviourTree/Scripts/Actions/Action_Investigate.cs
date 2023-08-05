using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_Investigate : ActionNode
{
    protected override void OnStart() {
        
    }

    protected override void OnStop() {
        context.NpcController.RemoveCurrentSensorySource();
        blackboard.currentSensorySource = null;
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
