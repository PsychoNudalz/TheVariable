using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_SetTaskToTarget : ActionNode
{
    protected override void OnStart()
    {
        if (context.NpcController.HasTasksQueued())
        {
            blackboard.moveToPosition = context.NpcController.GetCurrentTask().Position;
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
