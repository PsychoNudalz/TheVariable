using System;
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
            switch (cameraInvestigationMode)
            {
                case CameraInvestigationMode.None:
                    cameraToLock.SetInvestigationMode(cameraInvestigationMode);
                    break;
                case CameraInvestigationMode.Investigated:
                    cameraToLock.Set_Investigate(true);
                    break;
                case CameraInvestigationMode.Spotted:
                    cameraToLock.SetInvestigationMode(cameraInvestigationMode);
                    break;
            }

        }
        else if (blackboard.player_LastKnown_Camera)
        {
            cameraToLock = blackboard.player_LastKnown_Camera;
            switch (cameraInvestigationMode)
            {
                case CameraInvestigationMode.None:
                    cameraToLock.SetInvestigationMode(cameraInvestigationMode);
                    break;
                case CameraInvestigationMode.Investigated:
                    cameraToLock.Set_Investigate(true);
                    break;
                case CameraInvestigationMode.Spotted:
                    cameraToLock.SetInvestigationMode(cameraInvestigationMode);
                    break;
            }

        }
        
        else
        {
            return State.Failure;
        }

        return State.Success;
    }
}
