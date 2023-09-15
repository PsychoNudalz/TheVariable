using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_SSChange : DecoratorNode
{
    private SensorySource initialSS = null;
    protected override void OnStart()
    {
        initialSS = blackboard.currentSensorySource;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (initialSS!=null&&!initialSS.Equals(blackboard.currentSensorySource))
        {
            Abort();
            return State.Failure;
        }
        return child.Update();
    }
}
