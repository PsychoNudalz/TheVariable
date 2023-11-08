using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_SetHackingCameraToSS : ActionNode
{
    [SerializeField]
    private bool overrideCurrentSS = false;
    protected override void OnStart()
    {
        CameraController closestHackingCamera = context.NpcController.FindSS_ClosestCamera_Hacking();
        if (closestHackingCamera)
        {

            if (closestHackingCamera.ConnectedSo is CameraObject co)
            {
                SensorySource_Visual ssv = npcController.AddCameraToSS(co, overrideCurrentSS);
                if (ssv.Equals(npcController.GetCurrentSS))
                {
                    // blackboard.currentSensorySource = npcController.GetCurrentSS;
                    blackboard.cameraToInvestigate = closestHackingCamera;
                }
            }else if (closestHackingCamera.ConnectedSo is NpcObject npc)
            {
                SensorySource_Visual ssv = npcController.AddNPCToSS(npc, overrideCurrentSS);
                if (ssv.Equals(npcController.GetCurrentSS))
                {
                    // blackboard.currentSensorySource = npcController.GetCurrentSS;
                    blackboard.cameraToInvestigate = closestHackingCamera;
                }
            }
            else
            {
                started = false;
            }
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
