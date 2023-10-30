using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Check_General : DecoratorNode
{
    [Serializable]
    enum DecoratorCheckType
    {
        Alive,
        AlertState,
        Detection
    }

    [SerializeField]
    private DecoratorCheckType[] checkType = new[] { DecoratorCheckType.Alive };

    [SerializeField]
    private NPC_AlertState[] alertStates = Array.Empty<NPC_AlertState>();

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        //Testing
        // if (alertStates.Length > 0 && alertStates[0] == NPC_AlertState.Alert)
        // {
        //     Debug.Log("Test");
        // }
        //Testing

        State checkState = State.Success;
        foreach (DecoratorCheckType type in checkType)
        {
            checkState = EvaluateType(type);
            if (checkState == State.Failure)
            {
                Abort();
                return checkState;
            }
        }

        if (child != null)
        {
            State childState = child.Update();
            if (childState == State.Failure)
            {
                Abort();
            }
            return childState;
        }
        else
        {
            return State.Success;
        }
    }

    private State EvaluateType(DecoratorCheckType type)
    {
        switch (type)
        {
            case DecoratorCheckType.Alive:
                if (IsAlive() == State.Failure)
                {
                    return State.Failure;
                }

                break;
            case DecoratorCheckType.Detection:
                Debug.LogError("Detection check not implemented");
                break;
            case DecoratorCheckType.AlertState:
                if (!AllowedState(alertStates))
                {
                    return State.Failure;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return State.Success;
    }
}