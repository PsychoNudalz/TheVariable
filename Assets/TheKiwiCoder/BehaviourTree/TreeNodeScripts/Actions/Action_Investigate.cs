using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Investigating the current Sensory Source, increases alert value if player is nearby or controlling the investigated camera
/// Failure if the player if found/ state change
/// </summary>
[System.Serializable]
public class Action_Investigate : ActionNode
{
    [FormerlySerializedAs("investigateTimeRange")]
    [SerializeField]
    private Vector2 investigate_TimeRange = new Vector2(3, 10);

    [SerializeField]
    float alertBuildup = 1f;

    [SerializeField]
    private float decayAmount = 1f;

    [SerializeField]
    private bool returnFailureOnTimeExpire = true;

    private float investigate_Time = 0;

    private float investigate_StartTime = 0;
    private bool isPlayerSpotted = false;
    private SensorySource SSToInvestigate;
    
    bool hasShowedTutorial = false;


    // private bool skipRemoveSS = false;

    protected override void OnStart()
    {
        // blackboard.currentSensorySource = context.NpcController.GetCurrentSS;
        investigate_StartTime = Time.time;
        investigate_Time = Random.Range(investigate_TimeRange.x, investigate_TimeRange.y);
        isPlayerSpotted = false;
        if (blackboard.currentSensorySource is { SmartObject: CameraObject co })
        {
            blackboard.SetCameraToInvestigate(co.CameraController);
        }

        if (blackboard.currentSensorySource is { SmartObject: NpcObject npc })
        {
            blackboard.SetCameraToInvestigate(npc.Camera);
        }

        // if (blackboard.cameraToInvestigate)
        // {
        //     blackboard.cameraToInvestigate.Set_Investigate(true);
        // }

        SSToInvestigate = npcController.GetCurrentSS;

        Update_LastKnown(blackboard.CameraToInvestigate);
        if (!hasShowedTutorial)
        {
            TutorialManager.Display_FirstTime(TutorialEnum.SpotAndLockdown);
            TutorialManager.Display_FirstTime(TutorialEnum.LockdownImmunity);
            hasShowedTutorial = true;
        }
        // skipRemoveSS = true;
    }

    protected override void OnStop()
    {
        if (started)
        {
            // if (!skipRemoveSS)
            // {

            if (!isPlayerSpotted && SSToInvestigate.Equals(npcController.GetCurrentSS))
            {
                npcController.RemoveCurrentSensorySource();
            }
            // }

            if (blackboard.CameraToInvestigate && !isPlayerSpotted)
            {
                blackboard.CameraToInvestigate.Set_Investigate(false);
                blackboard.SetCameraToInvestigate(null);
            }
        }
    }

    protected override State OnUpdate()
    {
        // if (!blackboard.cameraToInvestigate)
        // {
        //     return State.Failure;
        // }
        if (Time.time - investigate_StartTime <= investigate_Time)
        {
            //TODO: I know setting this very second is bad, but i dont have time to optimise it
            if (blackboard.CameraToInvestigate)
            {
                blackboard.CameraToInvestigate.Set_Investigate(true);
            }

            //While the AI is investigating
            if (blackboard.CameraToInvestigate && blackboard.CameraToInvestigate.IsDetectable)
            {
                // if(blackboard.currentSensorySource)

                NPC_AlertState returnState = context.NpcController.Update_AlertValue(alertBuildup);

                Update_LastKnown(blackboard.CameraToInvestigate);
                //If the NPC spots the player
                //TODO: might change this to compare to Spotted state
                if (returnState != blackboard.alertState)
                {
                    ChangeAlertState(returnState, false);
                    isPlayerSpotted = true;
                    // skipRemoveSS = true;
                    return State.Failure;
                }
            }

            blackboard.currentSensorySource.DecayStrength(Time.deltaTime * decayAmount);
            if (returnFailureOnTimeExpire)
            {
                return State.Failure;
            }
            else
            {
                return State.Running;
            }
        }

        // skipRemoveSS = true;
        isPlayerSpotted = false;
        return State.Success;
    }
}