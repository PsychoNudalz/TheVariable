using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObject : SmartObject
{
    private NpcController controller;

    [SerializeField]
    private CameraController cameraController;

    public NpcController Controller => controller;
    public CameraController Camera => cameraController;

    protected override void AwakeBehaviour()
    {
        controller = GetComponent<NpcController>();
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
