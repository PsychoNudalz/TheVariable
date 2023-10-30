using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Selector_HasHealth : Selector_TrueFalse
{
    public float healthThreshold = 0;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        blackboard.health = context.NpcController.Health;
        return TrueOrFalse(blackboard.health>healthThreshold);
    }
}
