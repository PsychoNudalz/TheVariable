using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_Dead : ActionNode
{
    protected override void OnStart() {
        if (IsAlive() == State.Success)
        {
            started = false;
        }
        else
        {
            context.NpcController.PlayAnimation(NpcAnimation.Dead);
            context.agent.enabled = false;
        }
    }

    protected override void OnStop() {
        context.agent.enabled = true;

    }

    protected override State OnUpdate() {

        if (!started)
        {
            return State.Failure;
        }
        return State.Running;
    }
}
