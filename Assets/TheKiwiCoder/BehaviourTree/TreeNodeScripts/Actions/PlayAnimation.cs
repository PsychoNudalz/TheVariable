using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlayAnimation : ActionNode
{
    public NpcAnimation animation;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        try
        {
            context.NpcController.PlayAnimation(animation);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return State.Success;

        }
        return State.Success;
    }
}
