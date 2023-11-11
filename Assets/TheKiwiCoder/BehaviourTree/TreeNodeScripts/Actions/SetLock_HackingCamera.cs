using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetLock_HackingCamera : ActionNode
{
    protected override void OnStart()
    {
        CameraController cc = context.NpcController.FindSS_ClosestCamera_Hacking();
        if (cc && cc.ConnectedSo is CameraObject  or NpcObject)
        {
            blackboard.cameraToLock = cc;

        }
        else
        {
            started = false;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!started)
        {
            return State.Failure;
        }

        return State.Success;
    }
}
