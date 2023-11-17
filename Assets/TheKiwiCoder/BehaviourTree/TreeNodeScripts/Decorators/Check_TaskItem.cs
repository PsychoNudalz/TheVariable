using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Task;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_TaskItem : DecoratorNode
{
    private TaskEvent currentTask;

    protected override void OnStart()
    {
        currentTask = context.NpcController.GetCurrentTask();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.NpcController.CanStartCurrentTask(out ItemName[] missingItems) ||
            currentTask.HasItem(blackboard.pickedUpItem))
        {
            return child.Update();
        }
        else
        {
            blackboard.missingItem = missingItems[0];
            return State.Failure;
        }
    }
}