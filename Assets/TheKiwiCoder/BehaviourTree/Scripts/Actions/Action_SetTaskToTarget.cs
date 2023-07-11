using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_SetTaskToTarget : ActionNode
{
    protected override void OnStart()
    {
        blackboard.moveToPosition = context.npcData.GetCurrentTask().Position;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
