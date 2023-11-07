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

    // private bool skipRemoveSS = false;

    protected override void OnStart()
    {
        blackboard.currentSensorySource = context.NpcController.GetCurrentSS;
        investigate_StartTime = Time.time;
        investigate_Time = Random.Range(investigate_TimeRange.x, investigate_TimeRange.y);
        isPlayerSpotted = false;
        if (blackboard.currentSensorySource is { SmartObject: CameraObject co })
        {
            blackboard.cameraToInvestigate = co.CameraController;
        }

        if (blackboard.currentSensorySource is { SmartObject: NpcObject npc })
        {
            blackboard.cameraToInvestigate = npc.Camera;
        }

        if (blackboard.cameraToInvestigate)
        {
            blackboard.cameraToInvestigate.Set_Investigate(true);
        }

        Update_LastKnown(blackboard.cameraToInvestigate);
        // skipRemoveSS = true;
    }

    protected override void OnStop()
    {
        if (started)
        {
            // if (!skipRemoveSS)
            // {
            if (blackboard.currentSensorySource.Equals(npcController.GetCurrentSS))
            {
                context.NpcController.RemoveCurrentSensorySource();
            }

            if (!isPlayerSpotted)
            {
                blackboard.currentSensorySource = null;
            }
            // }

            if (blackboard.cameraToInvestigate)
            {
                blackboard.cameraToInvestigate.Set_Investigate(false);
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
            //While the AI is investigating
            if (blackboard.cameraToInvestigate && blackboard.cameraToInvestigate.IsDetectable)
            {
                // if(blackboard.currentSensorySource)

                NPC_AlertState returnState = context.NpcController.Update_AlertValue(alertBuildup);

                Update_LastKnown(blackboard.cameraToInvestigate);
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
        return State.Success;
    }
}