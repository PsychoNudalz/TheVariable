using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;


/// <summary>
/// Check if the task is free, success if free, fail if busy
/// </summary>
[System.Serializable]
public class Check_TaskFree : ActionNode
{
    public float distance = 2f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float dist = Vector3.Distance(agent_Position, context.NpcController.GetCurrentTask().Position);
        bool b = dist >= distance ||
                 (dist < distance &&
                  context.NpcController.IsTaskObjectFree());

        if (b)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}