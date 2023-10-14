using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_ClearSS : ActionNode
{
    protected override void OnStart()
    {
        npcController.RemoveCurrentSensorySource();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
