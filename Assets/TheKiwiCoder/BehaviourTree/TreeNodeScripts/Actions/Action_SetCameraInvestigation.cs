using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_SetCameraInvestigation : ActionNode
{
    [SerializeField]
    private CameraInvestigationMode cameraInvestigationMode;
    [SerializeField]
    private CameraController cameraToLock;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.CameraToInvestigate)
        {
            cameraToLock = blackboard.CameraToInvestigate;
            cameraToLock.SetInvestigationMode(cameraInvestigationMode);

        }
        else if (blackboard.player_LastKnown_Camera)
        {
            cameraToLock = blackboard.player_LastKnown_Camera;
            cameraToLock.SetInvestigationMode(cameraInvestigationMode);

        }
        
        else
        {
            return State.Failure;
        }

        return State.Success;
    }
}
