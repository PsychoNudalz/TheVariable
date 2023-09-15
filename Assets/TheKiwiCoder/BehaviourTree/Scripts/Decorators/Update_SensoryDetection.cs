using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Update_SensoryDetection : DecoratorNode
{
    [SerializeField]
    private bool returnFailureOnStateChange = false;


    [SerializeField]
    private bool returnTrueAfterUpdate = false;

    [SerializeField]
    private bool detectPlayerControl = false;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        SmartObject foundCamera = context.NpcController.Update_SensoryController(out SensorySource ss,detectPlayerControl);
        NPC_AlertState returnState;
        if (foundCamera)
        {
            returnState = context.NpcController.Update_AlertValue(foundCamera);
            if (ss != null)
            {
                blackboard.currentSensorySource = ss;
            }
        }
        else
        {
            returnState = context.NpcController.Update_AlertValue();
        }

        if (returnTrueAfterUpdate)
        {
            return State.Success;
        }

        if (returnState != blackboard.alertState)
        {
            ChangeAlertState(returnState, false);

            //Update 
            
            if (returnFailureOnStateChange)
            {
                Abort();
                return State.Failure;
            }
        }

        return child.Update();
    }
}
