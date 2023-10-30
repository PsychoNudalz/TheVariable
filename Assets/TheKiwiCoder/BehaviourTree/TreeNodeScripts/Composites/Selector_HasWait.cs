using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Selector_HasWait : Selector_TrueFalse
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return TrueOrFalse(blackboard.flag_wait);
    }
}
