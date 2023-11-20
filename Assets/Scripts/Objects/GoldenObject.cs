using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GoldenObject : SmartObject
{

    protected override void AwakeBehaviour()
    {
        if (renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
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

    public override void Hack_ChangeMaterial()
    {
        base.Hack_ChangeMaterial();
        SoundManager.PlayGlobal(SoundGlobal.CollectData);
    }
}
