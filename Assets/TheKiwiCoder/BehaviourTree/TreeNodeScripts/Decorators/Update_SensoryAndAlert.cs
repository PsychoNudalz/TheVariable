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
    private bool failureOnStateChange = false;

    [SerializeField]
    private bool successAfterUpdate = false;

    [SerializeField]
    private bool failureOnNewCameraDetected;


    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // SmartObject foundCamera = context.NpcController.Evaluate_Senses(out SensorySource ss,detectPlayerControl);
        NPC_AlertState returnState;
        if (blackboard.hackingCameras.Length > 0)
        {
            returnState = context.NpcController.Update_AlertValue(AlertValue_Increase);
            // if(blackboard.hackingCameras[0].)
            Update_LastKnown(blackboard.hackingCameras[0]);
            if (failureOnNewCameraDetected)
            {
                if (blackboard.currentSensorySource == null || blackboard.currentSensorySource is SensorySource_Audio ||
                    !(blackboard.currentSensorySource.SmartObject.Equals(blackboard.player_LastKnown_Camera
                        .ConnectedSo)))
                {
                    return State.Failure;
                }
            }
        }
        else
        {
            returnState = context.NpcController.Update_AlertValue(AlertValue_Decrease);
        }

        if (successAfterUpdate)
        {
            return State.Success;
        }


        if (returnState != blackboard.alertState)
        {
            ChangeAlertState(returnState, false);

            //Update 

            if (failureOnStateChange)
            {
                Abort();
                return State.Failure;
            }
        }

        return child.Update();
    }
}