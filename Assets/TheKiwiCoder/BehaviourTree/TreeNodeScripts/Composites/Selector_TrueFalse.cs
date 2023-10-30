using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;


/// <summary>
/// Based on comparing data, true: left node. false: right node
/// </summary>
[System.Serializable]
public class Selector_TrueFalse : CompositeNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

    protected virtual State TrueOrFalse(bool condition)
    {
        if (condition)
        {
            if (children.Count > 1)
            {
                // if (RightNode.started || RightNode.IsRunning)
                // {
                //     RightNode.Abort();
                // }
                RightNode.Abort();

            }

            return LeftNode.Update();
        }
        else
        {
            // if (LeftNode.started || LeftNode.IsRunning)
            // {
            //     LeftNode.Abort();
            // }
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

        return State.Failure;
    }
}