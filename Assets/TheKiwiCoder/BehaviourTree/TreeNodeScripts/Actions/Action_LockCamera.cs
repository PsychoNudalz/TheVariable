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
        if (blackboard.player_LastKnown_Camera)
        {
            cameraToLock = blackboard.player_LastKnown_Camera;
            npcController.MoveTransform(cameraToLock.InteractPosition, cameraToLock.InteractRotation,
                NpcAnimation.Interact);
        }
        else if (blackboard.cameraToInvestigate)
        {
            cameraToLock = blackboard.cameraToInvestigate;
            npcController.MoveTransform(cameraToLock.InteractPosition, cameraToLock.InteractRotation,
                NpcAnimation.Interact);
        }
        else
        {
            started = false;
            return;
        }
    }

    protected override void OnStop()
    {
        if (started)
        {
            blackboard.cameraToInvestigate = null;
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