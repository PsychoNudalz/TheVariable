using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;
using TheKiwiCoder;


/// <summary>
/// Queries task and set found task object to target location
/// </summary>
[System.Serializable]
public class Action_SetTaskToTarget : ActionNode
{
    protected override void OnStart()
    {
        if (context.NpcController.HasTasksQueued())
        {
            TaskEvent currentTask = context.NpcController.GetCurrentTask();
            TaskSmartObject foundTask = TaskManager.QueryTask(agent_Position,currentTask.TaskDescription);
            if (foundTask)
            {
                currentTask.SetTaskObject(foundTask);
                blackboard.targetPosition = currentTask.Position;

            }

            else
            {
                started = false;
            }
        }
        else
        {
            started = false;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!started)
        {
            return State.Failure;
        }
        return State.Success;
    }
}
