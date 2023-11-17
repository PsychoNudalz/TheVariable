using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToPosition : ActionNode
{
    // public float speed = 5;
    public float stoppingDistance = 0.5f;

    public bool updateRotation = true;

    // public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public bool playIdleAnimationOnStop = true;
    float pendingTimeOut = 2f;

    private bool samePositionHalt = false;
    public float samePosition_TimeOut = 1f;
    float samePosition_TimeOut_Now = 1f;
    public float samePosition_ResetTime = 1f;
    float samePosition_ResetTime_Now = 1f;
    float samePosition_Distance = .5f;
    private Vector3 lastPosition;

    protected override void OnStart()
    {
        context.agent.enabled = true;
        context.agent.isStopped=false;

        context.agent.stoppingDistance = stoppingDistance;
        // context.agent.speed = speed;
        context.agent.destination = blackboard.targetPosition;
        context.agent.updateRotation = updateRotation;
        // context.agent.acceleration = acceleration;
        // moveDelay_TimeNow = moveDelay;
        if (Vector3.Distance(blackboard.targetPosition, agent_Position) > tolerance)
        {
            context.NpcController.PlayAnimation(NpcAnimation.Walk);
        }

        pendingTimeOut = 2f;
        
        samePosition_ResetTime_Now = Random.Range(samePosition_ResetTime,samePosition_ResetTime*1.5f);
        samePosition_TimeOut_Now = Random.Range(samePosition_TimeOut,samePosition_TimeOut*1.5f);
        samePosition_Distance = .5f * context.agent.speed * Time.deltaTime;

        
    }

    protected override void OnStop()
    {
        if (started)
        {
            started = false;
        }

        context.agent.enabled = true;
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(agent_Position, lastPosition) < samePosition_Distance)
        {
            samePosition_TimeOut_Now -= Time.deltaTime;
            if (samePosition_TimeOut_Now < 0)
            {
                context.NpcController.PlayAnimation(NpcAnimation.Idle);
                DisableAgent();
                samePosition_ResetTime_Now -= Time.deltaTime;
                samePositionHalt = true;
                if (samePosition_ResetTime_Now < 0)
                {
                    samePositionHalt = false;
                    EnableAgent();
                }
                else
                {
                    return State.Running;
                }
            }
        }

        else
        {
            samePosition_ResetTime_Now = Random.Range(samePosition_ResetTime,samePosition_ResetTime*1.5f);
            samePosition_TimeOut_Now = Random.Range(samePosition_TimeOut,samePosition_TimeOut*1.5f);
        }


        if (context.agent.pathPending)
        {
            pendingTimeOut -= Time.deltaTime;
            if (pendingTimeOut < 0)
            {
                return State.Failure;
            }

            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            if (playIdleAnimationOnStop)
            {
                context.NpcController.PlayAnimation(NpcAnimation.Idle);
            }

            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        lastPosition = agent_Position;
        return State.Running;
    }

    void EnableAgent()
    {
        context.agent.enabled = true;
    }

    void DisableAgent()
    {
        context.agent.enabled = false;
    }
}