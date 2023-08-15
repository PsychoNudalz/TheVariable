using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class A_Update_SensoryDetection : ActionNode
{
    [SerializeField]
    private bool returnFailureOnDetected = false;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float f = context.NpcController.AlertUpdateBehaviour();
        if (returnFailureOnDetected & f >= 1)
        {
            return State.Failure;
        }

        return State.Running;
    }
}