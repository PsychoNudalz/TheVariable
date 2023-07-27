using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

/// <summary>
/// if the AI has the missing item
/// </summary>
[System.Serializable]
public class Selector_HasItemPickUp : Selector_TrueFalse
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return TrueOrFalse(blackboard.pickedUpItem && blackboard.pickedUpItem.Equals(blackboard.missingItem));
    }
}