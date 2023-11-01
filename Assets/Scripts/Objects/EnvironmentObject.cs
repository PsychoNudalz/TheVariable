using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : SmartObject
{
    [Header("Environmental Component")]
    [SerializeField]
    private DoorController doorController;
    

    protected override void AwakeBehaviour()
    {
    }

    protected override void StartBehaviour()
    {
    }

    protected override void UpdateBehaviour()
    {
    }

    public override void Interact(NpcController npc)
    {
        Debug.LogWarning($"{name}: should not be able to interact");
    }

    public void Hack_LockOpen()
    {
        if (!doorController)
        {
            Debug.LogError($"{name}: should not be able to Lock Open");
            return;
        }
        doorController.LockOpen();
    }
}
