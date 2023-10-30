using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_Alive : DecoratorNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        State alive = IsAlive();
        if (alive == State.Failure)
        {
            return State.Failure;
        }
        return child.Update();
    }
}
