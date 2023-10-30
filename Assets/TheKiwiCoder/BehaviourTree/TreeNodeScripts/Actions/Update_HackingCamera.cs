using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class Update_HackingCamera : ActionNode
{
    [SerializeField]
    private bool returnFailureOnStateChange = false;


    [SerializeField]
    private bool returnTrueAfterUpdate = false;
    
    [SerializeField]
    private bool detectPlayerControl = false;
    // [SerializeField]
    // private bool returnFailureOnDetected = false;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.hackingCameras = context.NpcController.FindSS_HackingCameras();
        return State.Running;
    }
}