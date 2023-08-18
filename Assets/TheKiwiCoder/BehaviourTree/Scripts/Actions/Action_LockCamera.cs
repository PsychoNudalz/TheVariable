using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_LockCamera : ActionNode
{
    [SerializeField]
    private float lockDuration = 5f;
    [SerializeField]
    private CameraObject cameraToLock;
    

    protected override void OnStart()
    {
        if (blackboard.currentSensorySource is {SmartObject: CameraObject co})
        {
            blackboard.cameraToLock = co;
            cameraToLock = blackboard.cameraToLock;
        }
        else
        {
            started = false;
            return;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!started || !cameraToLock)
        {
            return State.Failure;
        }
        
        cameraToLock.Set_Lock(true,lockDuration);

        if (cameraToLock.IsPlayerControl)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}