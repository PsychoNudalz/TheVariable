using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObject : SmartObject
{
    private NpcController controller;

    [Header("Eye Camera")]
    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private Transform eyeLocation;
    public NpcController Controller => controller;
    public CameraController Camera => cameraController;

    protected override void AwakeBehaviour()
    {
        controller = GetComponent<NpcController>();
        if (cameraController)
        {
            if (eyeLocation)
            {
                cameraController.transform.SetParent(eyeLocation);
                cameraController.transform.localPosition = new Vector3();
            }
        }
    }

    protected override void StartBehaviour()
    {
    }

    protected override void UpdateBehaviour()
    {
    }

    public override void Interact(NpcController npc)
    {
    }

    public void Hack_ClearTasks()
    {
        controller.ClearTasks();
    }

    public void Hack_StraightKill()
    {
        controller.TakeDamage(10000f);
    }

    public void Hack_SetAlertState(NPC_AlertState state)
    {
        controller.Override_AlertValue(state);
        controller.Set_AlertState(state);
    }


}
