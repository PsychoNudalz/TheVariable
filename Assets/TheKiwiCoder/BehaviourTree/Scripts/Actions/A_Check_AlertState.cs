using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class A_Check_AlertState : ActionNode
{
    [SerializeField]
    private NPC_AlertState[] alertStates = Array.Empty<NPC_AlertState>();

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if (!AllowedState(alertStates))
        {
            return State.Failure;
        }
        return State.Running;
    }


}
