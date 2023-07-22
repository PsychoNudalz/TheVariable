using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObject : SmartObjects
{
    private NpcController controller;
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

    public override void OnSelect_Enter()
    {
    }

    public override void OnSelect_Exit()
    {
    }
}
