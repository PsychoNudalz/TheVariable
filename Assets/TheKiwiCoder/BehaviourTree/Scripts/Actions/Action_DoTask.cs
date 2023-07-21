using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_DoTask : ActionNode
{
    private float taskDuration = 0;
    private TaskEvent currentTask;
    private bool hasStarted = false;
    protected override void OnStart()
    {
        currentTask = context.npcData.GetCurrentTask();
        taskDuration = context.npcData.StartCurrentTask();
        hasStarted = true;
    }

    protected override void OnStop() {
        if (hasStarted)
        {
            //To finish the task
            if (taskDuration > 0)
            {
                //if the task is incomplete
                Debug.Log($"Task: {context.npcData.GetCurrentTask().TaskName} interrupt.");
            }
            context.npcData.RemoveTask();

        }

        hasStarted = false;


    }

    protected override State OnUpdate() {
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
