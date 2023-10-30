using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Assign_SensorySource : ActionNode
{
    protected override void OnStart()
    {
        SensorySource ss = context.NpcController.GetCurrentSS;
        if (ss== null)
        {
            started = false;
        }

        blackboard.currentSensorySource = ss;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!started)
        {
            return State.Failure;
        }
        return State.Success;
    }
}
