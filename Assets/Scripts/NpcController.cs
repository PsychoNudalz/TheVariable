using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// Handles NPC
/// </summary>
public class NpcController : MonoBehaviour
{
    [FormerlySerializedAs("tasks")]
    [SerializeField]
    [Tooltip("Currently queued task. this is not the schedule")]
    private List<TaskEvent> taskQueue = new List<TaskEvent>();

    [SerializeField]
    [Tooltip("This is the schedule")]
    private List<TaskEvent> schedule = new List<TaskEvent>();

    [Header("Item")]
    [SerializeField]
    private ItemObject pickedUpItem;

    [SerializeField]
    private Transform itemHoldingPoint;

    [Header("Components")]
    [SerializeField]
    private NpcVisualController visualController;

    [SerializeField]
    NpcLifeSystem lifeSystem;


    [SerializeField]
    private BehaviourTreeRunner treeRunner;


    public List<TaskEvent> TaskQueue => taskQueue;

    public List<TaskEvent> Schedule => schedule;

    public NpcVisualController VisualController => visualController;

    public BehaviourTreeRunner TreeRunner => treeRunner;

    public Vector3 PickUpPosition => itemHoldingPoint.position;

    public float Health => lifeSystem.Health;

    private void Awake()
    {
        //Initialising schedule
        for (var i = 0; i < schedule.Count; i++)
        {
            schedule[i] = InitialiseTask(schedule[i]);
        }

        SortSchedule();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool HasTasks()
    {
        return taskQueue.Count > 0;
    }

    public bool IsTaskObjectFree()
    {
        if (HasTasks())
        {
            if (GetCurrentTask().TaskObject)
            {
                if (GetCurrentTask().TaskObject.InUse)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void AddTask(TaskEvent t)
    {
        taskQueue.Add(t);
    }

    public void RemoveTask(TaskEvent t)
    {
        taskQueue.Remove(t);
    }

    public void RemoveTask()
    {
        if (taskQueue.Count == 0)
        {
            return;
        }

        taskQueue.RemoveAt(0);
    }

    public TaskEvent GetCurrentTask()
    {
        if (!HasTasks())
        {
            return null;
        }
        else
        {
            return taskQueue[0];
        }
    }

    public bool CanStartCurrentTask(out ItemName[] items)
    {
        items = Array.Empty<ItemName>();
        if (!HasTasks())
        {
            return false;
        }
        else
        {
            return taskQueue[0].CanStartTask(out items);
        }
    }

    public float StartCurrentTask()
    {
        if (!HasTasks())
        {
            return -1;
        }
        else
        {
            if (taskQueue[0].HasObject)
            {
                taskQueue[0].TaskObject.Interact(this);
            }

            return taskQueue[0].Duration;
        }
    }

    public float FinishCurrentTask(bool isInterrupt = false)
    {
        if (!HasTasks())
        {
            return -1;
        }
        else
        {
            TaskEvent task = taskQueue[0];
            if (task.HasObject)
            {
                task.TaskObject.FinishTask(this, task, isInterrupt);
            }

            RemoveTask();
            return task.Duration;
        }
    }

    [ContextMenu("Add test task")]
    public void AddTestTask()
    {
        AddTask(new TaskEvent("Eat Food", 0, new Vector3(10, 0, 10), Random.Range(1f, 4f)));
    }

    [ContextMenu("Sort Schedule")]
    public void SortSchedule()
    {
        schedule = schedule.OrderBy(o => o.StartTime).ToList();
    }

    /// <summary>
    /// Initialise the position of the task
    /// </summary>
    /// <param name="taskEvent"></param>
    /// <returns></returns>
    TaskEvent InitialiseTask(TaskEvent taskEvent)
    {
        //setting the position to the task object's interaction point
        if (taskEvent is {HasObject: true, Position: {magnitude: <= .1f}})
        {
            taskEvent.Position = taskEvent.TaskObject.InteractPosition;
        }

        return taskEvent;
    }

    public bool UpdateTaskFromSchedule(int Tick)
    {
        bool flag = false;
        foreach (TaskEvent taskEvent in schedule)
        {
            if (taskEvent.StartTime.Equals(Tick))
            {
                AddTask(taskEvent);
                flag = true;
            }
        }

        return flag;
    }

    public void PickUpItem(ItemObject item)
    {
        pickedUpItem = item;
        item.PickUp(this, GetCurrentTask());
    }

    /// <summary>
    /// deposits the item to the task object
    /// Null means it deposits in to the current task
    /// </summary>
    /// <param name="taskObject"></param>
    public void DepositItem(TaskObject taskObject = null)
    {
        if (!taskObject)
        {
            taskObject = GetCurrentTask().TaskObject;
        }

        pickedUpItem.Deposit(taskObject);
        if (taskObject)
        {
            taskObject.Deposit(pickedUpItem);
        }
        else
        {
            Debug.LogWarning("Missing deposit point");
        }

        pickedUpItem = null;
    }

    public void DropItem()
    {

        if (pickedUpItem)
        {
            pickedUpItem.Drop();
        }
        pickedUpItem = null;
    }


    public void PlayAnimation(NpcAnimation npcAnimation)
    {
        if (visualController)
        {
            visualController.PlayAnimation(npcAnimation);
        }
    }
}