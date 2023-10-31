using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_RandomTask : ActionNode
{
    private TaskEvent randomTask;

    protected override void OnStart()
    {
        randomTask = npcController.PickRandomTask();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (randomTask==null)
        {
            return State.Failure;
        }
        npcController.AddTask(randomTask);
        return State.Success;
    }
}
