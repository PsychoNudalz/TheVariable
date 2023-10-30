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
        if (!blackboard.locatedItem)
        {
            started = false;
            return;
        }
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

    }

    protected override State OnUpdate()
    {
        if (!started)
        {
            return State.Failure;
        }
        State waitUpdate = Wait_Update();
        if (waitUpdate == State.Success)
        {
            if (started)
            {
                Wait_End();
                if (!context.NpcController.PickUpItem(blackboard.locatedItem))
                {
                    return State.Failure;
                }
                blackboard.pickedUpItem = blackboard.locatedItem;

            }
        }
        return waitUpdate;
    }
}
