using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_PlayWait : ActionNode
{
    float startTime => blackboard.wait_startTime;


    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float timeRemaining = Time.time - startTime;
        if (timeRemaining > blackboard.wait_duration)
        {
            blackboard.flag_wait = false;
            return State.Success;
        }

        return State.Running;
    }
}