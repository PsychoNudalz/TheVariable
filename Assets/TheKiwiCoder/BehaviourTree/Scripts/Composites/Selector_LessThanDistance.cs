using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Serialization;

/// <summary>
/// Compares distance between agent and target
/// runs left Node if within distance
/// </summary>
[System.Serializable]
public class Selector_LessThanDistance : Selector_TrueFalse
{
    public float distance;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return TrueOrFalse(Vector3.Distance(agent_Position, blackboard.moveToPosition) < distance);
    }
}