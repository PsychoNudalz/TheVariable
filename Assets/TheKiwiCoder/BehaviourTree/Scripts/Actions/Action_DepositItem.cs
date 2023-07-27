using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_DepositItem : ActionNode
{
    protected override void OnStart() {
        if (!blackboard.pickedUpItem)
        {
            return;
        }
        
        if (blackboard.pickedUpItem.Equals(blackboard.missingItem))
        {
            context.NpcController.DepositItem();
        }
        else
        {
            context.NpcController.DropItem();

        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
