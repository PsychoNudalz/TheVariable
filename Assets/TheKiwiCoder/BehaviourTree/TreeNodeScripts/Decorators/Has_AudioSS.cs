using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Has_AudioSS : DecoratorNode
{
    [SerializeField]
    private bool failure;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (npcController.GetCurrentSS != null&&npcController.GetCurrentSS is SensorySource_Audio)
        {
            return child.Update();

            
        }
        else
        {
            if (failure)
            {
                return State.Failure;
            }
            else
            {
                return State.Success;
            }
        }
    }
}
