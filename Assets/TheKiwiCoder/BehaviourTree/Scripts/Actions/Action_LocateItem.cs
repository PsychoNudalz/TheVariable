using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_LocateItem : ActionNode
{
    
    protected override void OnStart() {
        if (blackboard.missingItem!=ItemName.None)
        {
            blackboard.locatedItem = ItemManager.current.FindItem(blackboard.missingItem, agent_Position);
        }

        if (!blackboard.locatedItem)
        {
            started = false;
        }
        else
        {
            blackboard.targetPosition = blackboard.locatedItem.Position;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.locatedItem)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
