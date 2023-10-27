using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class A_Update_Sensory : ActionNode
{

    [SerializeField]
    private bool returnSuccessAfterUpdate = false;
    
    [SerializeField]
    private bool detectPlayerControl = false;
    // [SerializeField]
    // private bool returnFailureOnDetected = false;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        blackboard.hackingCameras = context.NpcController.FindSS_HackingCameras();
        // SmartObject foundCamera = context.NpcController.Evaluate_Senses(out SensorySource ss,detectPlayerControl);
        // NPC_AlertState returnState;
        // if (foundCamera)
        // {
        //     returnState = context.NpcController.Update_AlertValue(foundCamera);
        //     if (ss != null)
        //     {
        //         blackboard.currentSensorySource = ss;
        //     }
        // }
        // else
        // {
        //     returnState = context.NpcController.Update_AlertValue();
        // }
        //
        // if (returnTrueAfterUpdate)
        // {
        //     return State.Success;
        // }
        //
        // if (returnState != blackboard.alertState)
        // {
        //     ChangeAlertState(returnState, false);
        //
        //     //Update 
        //     
        //     if (returnFailureOnStateChange)
        //     {
        //         return State.Failure;
        //     }
        // }
        if (returnSuccessAfterUpdate)
        {
            // blackboard.hackingCameras = context.NpcController.FindSS_HackingCameras();

            return State.Success;
        }

        return State.Running;
    }
}