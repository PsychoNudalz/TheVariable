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
    private CameraController cameraToLock;


    protected override void OnStart()
    {
        if (blackboard.cameraToLock)
        {
            cameraToLock = blackboard.cameraToLock;
        }
        else if (blackboard.player_LastKnown_Camera)
        {
            cameraToLock = blackboard.player_LastKnown_Camera;
        }
        else if (blackboard.CameraToInvestigate)
        {
            cameraToLock = blackboard.CameraToInvestigate;
        }
        else
        {
            started = false;
            return;
        }

        if (cameraToLock.ConnectedSo is NpcObject)
        {
            npcController.MoveTransform(agent_Position, cameraToLock.InteractRotation,
                NpcAnimation.Interact);
        }

        cameraToLock.SetInvestigationMode(CameraInvestigationMode.Spotted);
    }

    protected override void OnStop()
    {
        if (started)
        {
            blackboard.SetCameraToInvestigate(null);
        }
    }

    protected override State OnUpdate()
    {
        if (!started || !cameraToLock)
        {
            Debug.LogError($"{name}: no camera to lock");
            return State.Failure;
        }

        cameraToLock.Set_Lock(true, lockDuration);

        if (cameraToLock.IsPlayerControl)
        {
            return State.Success;
        }
        else
        {
            Debug.Log($"{name}: Player moved before it can lock");

            return State.Failure;
        }
    }
}