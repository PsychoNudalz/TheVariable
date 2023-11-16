using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenObject : SmartObject
{
    [Header("More Data Effect")]
    [SerializeField]
    private Renderer[] renderers;
    [SerializeField]
    private Material goldlessMaterial;
    protected override void AwakeBehaviour()
    {
        if (renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
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

    public override void Hack_Data()
    {
        base.Hack_Data();

        List<Material> tempList = new List<Material>();

        if (goldlessMaterial)
        {
            foreach (Renderer r in renderers)
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
