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
            blackboard.targetPosition = blackboard.cameraToInvestigate.ConnectedSo.InteractPosition;
            blackboard.targetRotation = blackboard.cameraToInvestigate.ConnectedSo.InteractRotation;
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
