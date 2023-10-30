using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Action_ChangeAlertState : ActionNode
{
    public NPC_AlertState newState;
    public bool overrideAlertValue = true;

    protected override void OnStart()
    {
        ChangeAlertState(newState,overrideAlertValue);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}