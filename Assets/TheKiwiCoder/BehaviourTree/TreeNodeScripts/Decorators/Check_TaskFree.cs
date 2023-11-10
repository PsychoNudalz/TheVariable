using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_TaskFree : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.NpcController.IsTaskObjectFree())
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
        
    }
}