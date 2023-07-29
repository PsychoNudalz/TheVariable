using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_Dead : ActionNode
{
    protected override void OnStart() {
        context.NpcController.PlayAnimation(NpcAnimation.Dead);
        context.agent.enabled = false;
    }

    protected override void OnStop() {
        context.agent.enabled = true;

    }

    protected override State OnUpdate() {
        return State.Running;
    }
}
