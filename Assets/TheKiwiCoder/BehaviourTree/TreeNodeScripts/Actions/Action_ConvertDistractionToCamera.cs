using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

/// <summary>
/// Converts the currentSS audio to investigate the camera and not just investigate the object;
/// </summary>
[System.Serializable]
public class Action_ConvertDistractionToCamera : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (npcController.GetCurrentSS.SmartObject is CameraObject co)
        {
            npcController.AddCameraToSS(co, true);
            blackboard.SetCameraToInvestigate(co.CameraController);
            return State.Success;
        }else if (npcController.GetCurrentSS.SmartObject is NpcObject npc)
        {
            npcController.AddNPCToSS(npc, true);
            blackboard.SetCameraToInvestigate(npc.Camera);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
