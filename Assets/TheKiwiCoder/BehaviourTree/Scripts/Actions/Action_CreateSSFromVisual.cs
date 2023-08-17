using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_CreateSSFromVisual : ActionNode
{
    protected override void OnStart()
    {
        SmartObject so = context.NpcController.FindSS_HackingCamera();
        if (so)
        {
            context.NpcController.AddSensorySource(new SensorySource_Visual(so,100));
        }
        else
        {
            started = false;
            return;
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
