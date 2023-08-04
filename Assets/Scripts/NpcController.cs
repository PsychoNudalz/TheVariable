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


public enum NPC_AlertState
{
    Peace,
    Alert,
    Suspicious,
    Spotted,
    Hunt
}

/// <summary>
/// Handles NPC
/// </summary>
public class NpcController : MonoBehaviour
{
    [FormerlySerializedAs("tasks")]
    [SerializeField]
    private TaskEvent currentTask;

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

    public NPC_AlertState GetAlertState => treeRunner.tree.blackboard.alertState;

    public void SetAlertState(NPC_AlertState npcAlertState)
    {
        Debug.Log($"Change NPC state: {GetAlertState} --> {npcAlertState}");
        treeRunner.tree.blackboard.alertState = npcAlertState;
    }

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

    public bool HasTasksQueued()
    {
        if (currentTask != null && taskQueue.Count > 0)
        {
            UpdateCurrentTask();
        }

        return taskQueue.Count > 0;
    }

    public bool IsTaskObjectFree()
    {
        if (HasTasksQueued())
        {
            if (GetCurrentTask().TaskSmartObject)
            {
                if (GetCurrentTask().TaskSmartObject.InUse)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Adds a task and load task in to current if Null
    /// </summary>
    /// <param name="t"></param>
    public void AddTask(TaskEvent t)
    {
        taskQueue.Add(t);
        if (currentTask == null)
        {
            UpdateCurrentTask();
        }
    }

    private void UpdateCurrentTask()
    {
        currentTask = taskQueue[0];
    }

    public void RemoveTask(TaskEvent t)
    {
        taskQueue.Remove(t);
        if (taskQueue.Count > 0)
        {
            UpdateCurrentTask();
        }
        else
        {
            currentTask = null;
        }
    }

    public void RemoveTask()
    {
        if (taskQueue.Count == 0)
        {
            currentTask = null;
            return;
        }
        else
        {
            taskQueue.RemoveAt(0);
            if (taskQueue.Count > 0)
            {
                UpdateCurrentTask();
            }
            else
            {
                currentTask = null;
            }
        }
    }

    public TaskEvent GetCurrentTask()
    {
        if (!HasTasksQueued())
        {
            return null;
        }
        else
        {
            return currentTask;
        }
    }

    public bool CanStartCurrentTask(out ItemName[] items)
    {
        items = Array.Empty<ItemName>();
        if (!HasTasksQueued())
        {
            return false;
        }
        else
        {
            return currentTask.CanStartTask(out items);
        }
    }

    public float StartCurrentTask()
    {
        if (!HasTasksQueued())
        {
            return -1;
        }
        else
        {
            if (currentTask.HasObject)
            {
                currentTask.TaskSmartObject.Interact(this);
            }

            return currentTask.Duration;
        }
    }

    public float FinishCurrentTask(bool isInterrupt = false)
    {
        if (currentTask == null)
        {
            Debug.LogWarning($"{name} finish task is NULL");
        }

        TaskEvent task = currentTask;
        if (task.HasObject)
        {
            task.TaskSmartObject.FinishTask(this, task, isInterrupt);
        }

        RemoveTask();
        return task.Duration;
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
        if (taskEvent is { HasObject: true, Position: { magnitude: <= .1f } })
        {
            taskEvent.Position = taskEvent.TaskSmartObject.InteractPosition;
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
    /// <param name="taskSmartObject"></param>
    public void DepositItem(TaskSmartObject taskSmartObject = null)
    {
        if (!taskSmartObject)
        {
            taskSmartObject = GetCurrentTask().TaskSmartObject;
        }

        pickedUpItem.Deposit(taskSmartObject);
        if (taskSmartObject)
        {
            taskSmartObject.Deposit(pickedUpItem);
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

    public void ClearTasks()
    {
        DropItem();
        taskQueue = new List<TaskEvent>();
    }

    public void TakeDamage(float damage)
    {
        lifeSystem.TakeDamage(damage);
    }
}