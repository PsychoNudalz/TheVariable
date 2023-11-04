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
    private TaskEvent lastTaskEvent;
    protected override void OnStart()
    {
        if (context.NpcController.HasTasksQueued())
        {
            TaskEvent currentTask = context.NpcController.GetCurrentTask();
            TaskSmartObject foundTask = TaskManager.QueryTask(agent_Position,currentTask.TaskDescription,currentTask.TaskQueryType);
            if (foundTask)
            {
                currentTask.SetTaskObject(foundTask);
                blackboard.targetPosition = currentTask.Position;

            }

            else
            {
                //If not task object is found
                
                // Debug.LogWarning($"{name}: Failed to find task {currentTask}");
                lastTaskEvent = currentTask;
                started = false;
                
                //skips current task if it overlaps with next task if it can't find the task object
                TaskEvent peakNextTask = context.NpcController.PeakNextTask();
                if (peakNextTask!=null)
                {
                    if (TaskManager.Tick >= peakNextTask.StartTime)
                    {
                        context.NpcController.RemoveTask();
                    }
                }
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
