using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_SensorySource : ActionNode
{
    protected override void OnStart()
    {
        blackboard.moveToPosition = blackboard.currentSensorySource.Position;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}