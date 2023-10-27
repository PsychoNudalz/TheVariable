using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_CameraToInvestigate : ActionNode
{
    protected override void OnStart() {
        if (blackboard.cameraToInvestigate)
        {
            blackboard.moveToPosition = blackboard.cameraToInvestigate.InteractPosition;
        }
        else
        {
            started = false;
        }
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
