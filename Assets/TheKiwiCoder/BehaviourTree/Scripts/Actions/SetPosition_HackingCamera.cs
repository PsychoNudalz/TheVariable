using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_HackingCamera : ActionNode
{
    protected override void OnStart()
    {
        if (blackboard.hackingCameras.Length>0)
        {
            blackboard.moveToPosition = context.NpcController.FindSS_ClosestCamera_Hacking().InteractPosition;
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
