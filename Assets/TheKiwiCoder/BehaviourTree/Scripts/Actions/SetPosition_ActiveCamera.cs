using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_ActiveCamera : ActionNode
{
    protected override void OnStart()
    {
        CameraObject co = context.NpcController.FindSS_ClosestCamera_Active();
        if (co)
        {
            blackboard.moveToPosition = co.InteractPosition;

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
