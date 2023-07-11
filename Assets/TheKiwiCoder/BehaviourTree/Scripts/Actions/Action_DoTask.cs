using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
        taskDuration = context.npcData.GetCurrentTask().Duration;
        hasStarted = true;
    }

    protected override void OnStop() {
        if (hasStarted)
        {
            if (taskDuration > 0)
            {
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
