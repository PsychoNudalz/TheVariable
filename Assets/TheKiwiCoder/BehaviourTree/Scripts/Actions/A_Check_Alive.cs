using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class A_Check_Alive : ActionNode
{

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        State isAlive = IsAlive();
        if (isAlive == State.Failure)
        {
            return State.Failure;
        }
        return isAlive;
    }


}
