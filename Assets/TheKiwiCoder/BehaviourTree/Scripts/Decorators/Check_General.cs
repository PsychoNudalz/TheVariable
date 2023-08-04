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


        return child.Update();
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