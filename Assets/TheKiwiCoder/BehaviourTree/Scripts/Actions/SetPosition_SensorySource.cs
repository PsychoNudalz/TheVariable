using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetPosition_SensorySource : ActionNode
{
    protected override void OnStart()
    {
        blackboard.targetPosition = blackboard.currentSensorySource.Position;
        SmartObject so = blackboard.currentSensorySource.SmartObject;
        if (so )
        {
            blackboard.targetRotation = so.InteractRotation;

        }

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}