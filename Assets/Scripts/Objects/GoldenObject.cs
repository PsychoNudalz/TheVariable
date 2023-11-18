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

        List<Material> tempList = new List<Material>();

        if (goldlessMaterial)
        {
            foreach (MeshRenderer r in renderers)
            {
                tempList = new List<Material>();
                for (int i = 0; i < r.materials.Length; i++)
                {
                    tempList.Add(goldlessMaterial);
                }

                r.materials = tempList.ToArray();

            }
        }
    }
}
