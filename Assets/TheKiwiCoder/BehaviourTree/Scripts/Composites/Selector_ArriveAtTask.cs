using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Selector_ArriveAtTask : Selector_TrueFalse
{
    public float distance;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return TrueOrFalse(Vector3.Distance(agent_Position, context.npcData.GetCurrentTask().Position) < distance);

    }
}
