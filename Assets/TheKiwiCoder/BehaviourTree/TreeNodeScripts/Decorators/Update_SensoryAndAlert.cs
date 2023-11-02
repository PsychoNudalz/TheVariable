using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Update_SensoryAndAlert : DecoratorNode
{
    [SerializeField]
    private float AlertValue_Increase = 1f;

    [SerializeField]
    private float AlertValue_Decrease = -1f;
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
        // SmartObject foundCamera = context.NpcController.Evaluate_Senses(out SensorySource ss,detectPlayerControl);
        NPC_AlertState returnState;
        if (blackboard.hackingCameras.Length > 0)
        {
            returnState = context.NpcController.Update_AlertValue(AlertValue_Increase);
            blackboard.player_LastKnown_Camera = blackboard.hackingCameras[0];
            blackboard.player_LastKnown_Position = blackboard.player_LastKnown_Camera.Position;
            blackboard.player_LastKnown_Time = Time.time;
        }
        else
        {
            returnState = context.NpcController.Update_AlertValue(AlertValue_Decrease);
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
