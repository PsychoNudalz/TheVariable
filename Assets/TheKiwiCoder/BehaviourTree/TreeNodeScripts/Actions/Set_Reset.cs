using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Set_Reset : ActionNode
{
    [SerializeField]
    private bool reset = true;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        blackboard.resetFlag = reset;
        return State.Success;
    }
}
