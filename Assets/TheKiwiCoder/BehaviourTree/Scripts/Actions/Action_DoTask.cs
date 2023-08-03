using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;
using TheKiwiCoder;

/// <summary>
/// Does the task, success on completion, running while counting timer, fail if missing items
/// </summary>
[System.Serializable]
public class Action_DoTask : ActionNode
{
    private float taskDuration = 0;
    private TaskEvent currentTask;

    protected override void OnStart()
    {
        currentTask = context.NpcController.GetCurrentTask();
        if (context.NpcController.CanStartCurrentTask(out ItemName[] missingItems))
        {
            taskDuration = context.NpcController.StartCurrentTask();
            started = true;
        }
        else
        {
            blackboard.missingItem = missingItems[0];
            Debug.Log("Missing item for task");
            started = false;
        }
    }

    protected override void OnStop()
    {
        if (started)
        {
            //To finish the task

            if (taskDuration > 0)
            {
                //if the task is incomplete
                Debug.Log($"Task: {currentTask.TaskName} interrupt.");
            }

            context.NpcController.FinishCurrentTask(taskDuration > 0);
        }

        started = false;
    }

    protected override State OnUpdate()
    {
        if (!started)
        {
            return State.Failure;
        }

        if (!context.NpcController.HasTasksQueued())
        {
            return State.Failure;
        }

        if (IsAlive() == State.Failure)
        {
            return State.Failure;
        }

        if (taskDuration > 0)
        {
            taskDuration -= Time.deltaTime;
            state = State.Running;
            return State.Running;
        }

        else
        {
            state = State.Success;
            return State.Success;
        }
    }
}