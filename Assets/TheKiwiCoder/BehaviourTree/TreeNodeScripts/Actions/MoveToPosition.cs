using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToPosition : ActionNode
{
    // public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    // public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public bool playIdleAnimationOnStop = true;
    private const float moveDelay = 1f;
    private float moveDelay_TimeNow = 0;
    private float pendingTimeOut = 2f;
    protected override void OnStart()
    {
        context.agent.enabled = true;

        context.agent.stoppingDistance = stoppingDistance;
        // context.agent.speed = speed;
        context.agent.destination = blackboard.targetPosition;
        context.agent.updateRotation = updateRotation;
        // context.agent.acceleration = acceleration;
        moveDelay_TimeNow = moveDelay;
        context.NpcController.PlayAnimation(NpcAnimation.Walk);

    }

    protected override void OnStop()
    {
        if (started)
        {
            started = false;
            if (playIdleAnimationOnStop)
            {
                context.NpcController.PlayAnimation(NpcAnimation.Idle);
            }
        }
        context.agent.enabled = true;

    }

    protected override State OnUpdate()
    {
        // if (IsAlive() == State.Failure)
        // {
        //     return State.Failure;
        // }

        // if (moveDelay_TimeNow > 0)
        // {
        //     context.agent.enabled = false;
        //     moveDelay_TimeNow -= Time.deltaTime;
        //     if (moveDelay_TimeNow <= 0)
        //     {
        //         EnableAgent();
        //     }
        //     return State.Running;
        // }
        //
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
        
        return State.Running;
    }

    void EnableAgent()
    {
        context.agent.enabled = true;
    }
    
    
}