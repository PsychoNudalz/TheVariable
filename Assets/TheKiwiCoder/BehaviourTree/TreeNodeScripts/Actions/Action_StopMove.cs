using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_StopMove : MoveToPosition
{
    protected override void OnStart()
    {
        blackboard.targetPosition = agent_Position;
        context.agent.stoppingDistance = stoppingDistance;
        // context.agent.speed = speed;
        context.agent.updateRotation = updateRotation;
        // context.agent.acceleration = acceleration;
        context.agent.SetDestination(blackboard.targetPosition);
        if (playIdleAnimationOnStop)
        {
            context.NpcController.PlayAnimation(NpcAnimation.Idle);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Success;
    }
}