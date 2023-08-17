using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class Action_Investigate : ActionNode
{
    [FormerlySerializedAs("investigateTimeRange")]
    [SerializeField]
    private Vector2 investigate_TimeRange = new Vector2(3, 10);

    private float investigate_Time = 0;
    private float investigate_StartTime = 0;

    protected override void OnStart()
    {
        blackboard.currentSensorySource = context.NpcController.GetCurrentSS;
        investigate_StartTime = Time.time;
        investigate_Time = Random.Range(investigate_TimeRange.x, investigate_TimeRange.y);
    }

    protected override void OnStop()
    {
        context.NpcController.RemoveCurrentSensorySource();
        blackboard.currentSensorySource = null;
    }

    protected override State OnUpdate()
    {
        if (Time.time - investigate_StartTime <= investigate_Time)
        {
            //While the AI is investigating
            if (context.NpcController.FindSS_ActiveCameras().Length > 0)
            {
                context.NpcController.UpdateAlertValue(2f);
                NPC_AlertState returnState = context.NpcController.EvaluateAlertValue();

                if (returnState != blackboard.alertState)
                {
                    ChangeAlertState(returnState);
                    return State.Failure;
                }
            }

            return State.Running;
        }

        return State.Success;
    }
}