using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Move_TaskWait : MoveToPosition
{
    [Space(5)]
    public float taskWaitDistance = 2f;
    private bool wasNotFree = false;
    

    protected override State OnUpdate() {

        if (context.NpcController.GetCurrentTask() == null)
        {
            return State.Failure;
        }
        
        float dist = Vector3.Distance(agent_Position, context.NpcController.GetCurrentTask().Position);
        bool notFree = !(dist >= taskWaitDistance ||
                        (dist < taskWaitDistance &&
                         context.NpcController.IsTaskObjectFree()));
        if (notFree!=wasNotFree)
        {
            wasNotFree = notFree;
            if (wasNotFree)
            {
                context.agent.SetDestination(agent_Position);
                context.NpcController.PlayAnimation(NpcAnimation.Idle);
                
            }
            else
            {
                context.agent.SetDestination(blackboard.targetPosition);


            }
        }

        if (wasNotFree)
        {
            return State.Running;
        }


        return base.OnUpdate();
    }
}
