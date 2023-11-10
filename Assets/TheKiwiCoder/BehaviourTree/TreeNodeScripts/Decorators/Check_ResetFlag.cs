using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_ResetFlag : DecoratorNode
{
    [SerializeField]
    private bool FailureOnReset = false;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
        blackboard.resetFlag = false;
    }

    protected override State OnUpdate()
    {
        if (blackboard.resetFlag)
        {
            Abort();
            if (FailureOnReset)
            {
                return State.Failure;
            }
            else
            {
                return State.Success;
            }
        }

        return child.Update();
    }
}