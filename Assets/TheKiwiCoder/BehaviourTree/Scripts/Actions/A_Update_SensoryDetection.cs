using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class A_Update_SensoryDetection : ActionNode
{
    [SerializeField]
    private bool returnFailureOnDetected = false;

    
    [SerializeField]
    private bool returnTrueAfterUpdate = false;
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
        NPC_AlertState returnState = context.NpcController.AlertUpdateBehaviour();

        if (returnTrueAfterUpdate)
        {
            return  State.Success;
        }
        if (returnState != blackboard.alertState)
        {
            ChangeAlertState(returnState);
            
            if (returnFailureOnDetected)
            {
                return State.Failure;
            }
        }

        return State.Running;
    }
}