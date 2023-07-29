using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

/// <summary>
/// waiting and picking up an item

[System.Serializable]
public class Action_PickUpItem : Action_SetWait
{
    public float pickUpRange = 2f;


    protected override void OnStart() {
        if (Vector3.Distance(agent_Position, blackboard.locatedItem.Position) < pickUpRange)
        {
            context.NpcController.PlayAnimation(NpcAnimation.PickUp);
            Wait_Start();
        }
        else
        {
            started = false;
        }
    }

    protected override void OnStop() {
        if (started)
        {
            Wait_End();
            context.NpcController.PickUpItem(blackboard.locatedItem);
            blackboard.pickedUpItem = blackboard.locatedItem;

        }
    }

    protected override State OnUpdate() {
        return Wait_Update();
    }
}
