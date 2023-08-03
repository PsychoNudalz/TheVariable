using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_Alive : ActionNode
{
    public float healthThreshold = 0;

    protected override void OnStart() {
        blackboard.health = context.NpcController.Health;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.health > healthThreshold)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
