using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_DropItem : ActionNode
{
    protected override void OnStart() {
        if (!blackboard.pickedUpItem)
        {
            return;
        }
        context.NpcController.DropItem();
        blackboard.pickedUpItem = null;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
