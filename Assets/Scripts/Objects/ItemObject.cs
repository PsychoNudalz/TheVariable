using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;

public enum ItemState
{
    Idle,
    PickUp,
    Using
}

public enum ItemName
{
    None,
    Apple,
    Poison
}

public class ItemObject : SmartObject
{
    private ItemState itemState = ItemState.Idle;

    [Header("Item")]
    [SerializeField]
    private ItemName name = ItemName.None;

    [SerializeField]
    private GameObject modelGO;

    [SerializeField]
    private SoundAbstract sfx_PickUp;
    [SerializeField]
    private SoundAbstract sfx_Drop;

    public virtual bool IsFree => currentTask == null&&itemState == ItemState.Idle;
    private TaskEvent currentTask;
    public ItemName Name => name;

    public bool IsUsing => currentTask.Equals(null);

    protected override void AwakeBehaviour()
    {
    }

    protected override void StartBehaviour()
    {
    }

    protected override void UpdateBehaviour()
    {
    }

    /// <summary>
    /// Not for pick up, some items may have it's own interact behaviour
    /// </summary>
    /// <param name="npc"></param>
    public override void Interact(NpcController npc)
    {
    }

    public virtual bool PickUp(NpcController npc, TaskEvent taskEvent)
    {
        if (itemState != ItemState.Idle)
        {
            return false;
        }
        AssignTask(taskEvent);
        itemState = ItemState.PickUp;
        transform.parent = npc.transform;
        transform.position = npc.PickUpPosition;
        modelGO.SetActive(true);
        if (sfx_PickUp)
        {
            sfx_PickUp.PlayF();
        }
        return true;
    }

    public virtual void AssignTask(TaskEvent taskEvent)
    {
        currentTask = taskEvent;
        if (taskEvent != null)
        {
            Debug.Log($"{name} task assign: {taskEvent.TaskName}");
        }
        else
        {
            Debug.Log($"{name} task assign: null");
        }
    }

    public virtual void AssignTask()
    {
        currentTask = null;
        Debug.Log($"{name} task assign: null");
    }

    public virtual void Deposit(TaskSmartObject taskSmartObject)
    {
        if (taskSmartObject)
        {
            transform.parent = taskSmartObject.transform;
            modelGO.SetActive(false);
        }
        else
        {
            transform.parent = null;
        }
        itemState = ItemState.Using;

    }

    public virtual void Drop(Vector3 position = default)
    {
        AssignTask();
        itemState = ItemState.Idle;

        if (position.Equals(default))
        {
            transform.position = transform.parent.position;
        }
        else
        {
            transform.position = position;
        }

        transform.parent = null;
        modelGO.SetActive(true);
        if (sfx_Drop)
        {
            sfx_Drop.PlayF();
        }
    }

    public virtual void Destroy()
    {
        AssignTask();
        Destroy(gameObject);
    }

    public override bool Equals(object other)
    {
        if (other is ItemName n)
        {
            return name.Equals(n);
        }
        else
        {
            return base.Equals(other);
        }
    }

    public bool Equals(ItemName n)
    {
        return name.Equals(n);
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}