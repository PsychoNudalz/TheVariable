using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Selector_IsTaskFree : Selector_TrueFalse
{
    public float distance = 3f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float dist = Vector3.Distance(agent_Position, context.NpcController.GetCurrentTask().Position);
        return TrueOrFalse(dist >= distance ||
                           (dist < distance &&
                            context.NpcController.IsTaskObjectFree()));
    }
}