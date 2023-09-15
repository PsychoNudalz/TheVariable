using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Investigating the current Sensory Source, increases alert value if player is nearby or controlling the investigated camera
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
    private bool returnFailureOnTimeExpire = true;

    private float investigate_Time = 0;

    private float investigate_StartTime = 0;
    private bool isPlayerSpotted = false;

    protected override void OnStart()
    {
        blackboard.currentSensorySource = context.NpcController.GetCurrentSS;
        investigate_StartTime = Time.time;
        investigate_Time = Random.Range(investigate_TimeRange.x, investigate_TimeRange.y);
        isPlayerSpotted = false;
        if (blackboard.currentSensorySource is { SmartObject: CameraObject co })
        {
            blackboard.cameraToInvestigate = co;
        }
    }

    protected override void OnStop()
    {
        if (started)
        {
            context.NpcController.RemoveCurrentSensorySource();

            if (!isPlayerSpotted)
            {
                blackboard.currentSensorySource = null;
            }
        }
    }

    protected override State OnUpdate()
    {
        if (!blackboard.cameraToInvestigate)
        {
            return State.Failure;
        }
        if (Time.time - investigate_StartTime <= investigate_Time)
        {
            //While the AI is investigating
            if (blackboard.cameraToInvestigate&&blackboard.cameraToInvestigate.IsDetectable)
            {
                // if(blackboard.currentSensorySource)
                context.NpcController.UpdateAlertValue(alertBuildup);
                NPC_AlertState returnState = context.NpcController.EvaluateAlertValue();

                if (returnState != blackboard.alertState)
                {
                    ChangeAlertState(returnState, false);
                    isPlayerSpotted = true;
                    return State.Failure;
                }
            }

            if (returnFailureOnTimeExpire)
            {
                return State.Failure;
            }
            else
            {
                return State.Running;
            }
        }

        return State.Success;
    }
}