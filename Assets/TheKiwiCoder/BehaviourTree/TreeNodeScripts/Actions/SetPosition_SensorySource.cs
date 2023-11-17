using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Unity.Mathematics;

[System.Serializable]
public class SetPosition_SensorySource : ActionNode
{
    protected override void OnStart()
    {
        blackboard.targetPosition = blackboard.currentSensorySource.Position;
        SmartObject so = blackboard.currentSensorySource.SmartObject;
        if (so)
        {
            if (Vector3.Distance(so.InteractPosition, blackboard.targetPosition) > 1f)
            {
                Vector3 direction = (so.InteractPosition - blackboard.targetPosition).normalized;
                // float angle = Vector3.SignedAngle(Vector3.forward,direction,Vector3.up);
                blackboard.targetRotation =
                    quaternion.LookRotation(direction,Vector3.up);
            }
            else
            {
                blackboard.targetRotation = so.InteractRotation;
            }
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