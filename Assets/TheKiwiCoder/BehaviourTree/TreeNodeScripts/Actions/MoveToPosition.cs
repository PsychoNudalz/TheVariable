using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;


    private float pendingTimeOut = 2f;
    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.targetPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        started = false; //Assume false unless the update starts and the path is valid
    }

    protected override void OnStop()
    {
        if (started)
        {
            started = false;
            context.NpcController.PlayAnimation(NpcAnimation.Idle);
        }
    }

    protected override State OnUpdate()
    {
        // if (IsAlive() == State.Failure)
        // {
        //     return State.Failure;
        // }
        if (context.agent.pathPending)
        {
            pendingTimeOut -= Time.deltaTime;
            if (pendingTimeOut < 0)
            {
                pendingTimeOut = 2f;
                return State.Failure;
            }
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        if (!started)
        {
            context.NpcController.PlayAnimation(NpcAnimation.Walk);
            started = true;
        }

        return State.Running;
    }
}