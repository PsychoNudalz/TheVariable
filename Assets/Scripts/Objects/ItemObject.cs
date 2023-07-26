using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;


public enum ItemName
{
    Base
}
public class ItemObject : SmartObject
{
    [Header("Item")]
    [SerializeField]
    private ItemName name = ItemName.Base;

    private TaskEvent currentTask;
    public ItemName Name => name;

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
    }
}
