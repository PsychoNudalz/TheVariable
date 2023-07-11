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
public class Selector_LessThanDistance : CompositeNode
{
    [FormerlySerializedAs("Distance")]
    public float distance;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (children.Count > 0)
        {
            if (Vector3.Distance(agent_Position, blackboard.moveToPosition) < distance)
            {
                if (children.Count > 1)
                {
                    RightNode.Abort();
                }
                return LeftNode.Update();

            }
            else
            {
                LeftNode.Abort();
                if (children.Count > 1)
                {
                    return RightNode.Update();
                }
                else
                {
                    return State.Success;
                }
            }
        }

        return State.Failure;
    }
}
