using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetLock_CameraToInvestigate : ActionNode
{
    protected override void OnStart() {
        if (blackboard.CameraToInvestigate)
        {
            blackboard.cameraToLock = blackboard.CameraToInvestigate;

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
