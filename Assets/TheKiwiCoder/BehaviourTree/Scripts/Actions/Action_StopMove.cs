using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_StopMove : ActionNode
{
    
     float speed = 5;
     float stoppingDistance = 0.1f;
     bool updateRotation = true;
     float acceleration = 40.0f;
    protected override void OnStart()
    {
        blackboard.moveToPosition = agent_Position;
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        context.agent.SetDestination(blackboard.moveToPosition);
        context.NpcController.PlayAnimation(NpcAnimation.Idle);

        
    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() {

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }
        return State.Success;
    }
}
