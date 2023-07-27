using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_StopMove : ActionNode
{
    protected override void OnStart()
    {
        blackboard.moveToPosition = agent_Position;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
