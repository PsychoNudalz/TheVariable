using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_LockCamera : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!blackboard.cameraToLock && blackboard.cameraToInvestigate)
        {
            blackboard.cameraToLock = blackboard.cameraToInvestigate;
        }

        if (!blackboard.cameraToLock)
        {
            return State.Failure;
        }

        blackboard.targetPosition = blackboard.cameraToLock.InteractPosition;
        blackboard.targetRotation = blackboard.cameraToLock.InteractRotation;
        return State.Success;
    }
}