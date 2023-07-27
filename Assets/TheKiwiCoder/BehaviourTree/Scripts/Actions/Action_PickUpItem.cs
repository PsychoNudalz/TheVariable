using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_PickUpItem : ActionNode
{
    public float pickUpRange = 2f;

    protected override void OnStart() {
        if (Vector3.Distance(agent_Position, blackboard.locatedItem.Position) < pickUpRange)
        {
            //NPC controller picks up item
            context.NpcController.PickUpItem(blackboard.locatedItem);
            blackboard.pickedUpItem = blackboard.locatedItem;
        }
        else
        {
            started = false;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
