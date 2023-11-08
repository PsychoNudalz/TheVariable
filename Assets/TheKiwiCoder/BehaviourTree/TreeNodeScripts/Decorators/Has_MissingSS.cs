using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Has_MissingSS : DecoratorNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (npcController.GetCurrentSS != null)
        {
            return State.Success;
        }
        else
        {
            return child.Update();
        }
    }
}
