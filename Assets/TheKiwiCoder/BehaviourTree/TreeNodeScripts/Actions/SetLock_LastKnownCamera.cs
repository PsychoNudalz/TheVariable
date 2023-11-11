using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetLock_LastKnownCamera : ActionNode
{
    protected override void OnStart()
    {
        CameraController cc = blackboard.player_LastKnown_Camera;
        if (cc && cc.ConnectedSo is CameraObject or NpcObject)
        {
            if (!cc.Equals(blackboard.CameraToInvestigate))
            {
                Debug.LogWarning($"Last known camera miss match with investigate");
            }
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
