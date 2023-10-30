using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_HasHackingCamera : DecoratorNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.hackingCameras.Length > 0)
        {
            return child.Update();
        }
        else
        {
            return State.Failure;
        }
    }
}
