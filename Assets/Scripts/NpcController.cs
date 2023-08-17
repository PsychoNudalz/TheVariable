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
    Peace, // Follow schedule
    Alert, // Be scared and hide/hunt
    Suspicious, // Investigating
    Spotted, // Attacking
    Hunt // Looking for the player
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

    [Header("Alert State")]
    [SerializeField]
    private float alertValue = 0;

    [SerializeField]
    private float peaceToAlertSpeed = 2f;

    [SerializeField]
    private float alert_SuspiciousThresshold = .5f;

    [SerializeField]
    float alert_SpottedThresshold = 1f;


    [SerializeField]
    Transform alertPosition;

    [Header("Death")]
    [SerializeField]
    float deathSizeMultiplier = .5f;

    private Vector3 originalBodyCenter;
    float originalHeight;


    [Header("Components")]
    [SerializeField]
    private NpcVisualController visualController;

    [SerializeField]
    NpcLifeSystem lifeSystem;

    [SerializeField]
    private NpcSensoryController sensoryController;


    [SerializeField]
    private BehaviourTreeRunner treeRunner;

    [SerializeField]
    private UIController uiController;

    [SerializeField]
    private CapsuleCollider bodyCollider;

    public List<TaskEvent> TaskQueue => taskQueue;

    public List<TaskEvent> Schedule => schedule;

    public NpcVisualController VisualController => visualController;

    public BehaviourTreeRunner TreeRunner => treeRunner;

    public Vector3 PickUpPosition => itemHoldingPoint.position;

    public Vector3 AlertPosition => alertPosition.position;
    public float Health => lifeSystem.Health;
    public bool IsRoaming => blackboardAlertState is NPC_AlertState.Peace or NPC_AlertState.Alert;


    public SensorySource GetCurrentSS => sensoryController.GetCurrentSS;

    NPC_AlertState blackboardAlertState
    {
        get => treeRunner.tree.blackboard.alertState;
        set => treeRunner.tree.blackboard.alertState = value;
    }


    public void SetAlertState(NPC_AlertState npcAlertState, float alert = 0)
    {
        alertValue += alert;
        UpdateAlertValue(1f);
        Debug.Log($"Controller change NPC state from: {blackboardAlertState} --> {npcAlertState}");
        blackboardAlertState = npcAlertState;
    }

    public void SetMinAlertValue(NPC_AlertState npcAlertState)
    {
        // alertValue += alert;
        // UpdateAlertValue(1f);
        // Debug.Log($"Controller change NPC state from: {blackboardAlertState} --> {npcAlertState}");
        // blackboardAlertState = npcAlertState;
        float thresholdOffset = .1f;
        switch (npcAlertState)
        {
            case NPC_AlertState.Peace:
                alertValue = Math.Max(alertValue, 0);
                break;
            case NPC_AlertState.Alert:
                alertValue = Math.Max(alertValue, 0);

                break;
            case NPC_AlertState.Suspicious:
                alertValue = Math.Max(alertValue, alert_SuspiciousThresshold + thresholdOffset);

                break;
            case NPC_AlertState.Spotted:
                alertValue = Math.Max(alertValue, alert_SpottedThresshold + thresholdOffset);

                break;
            case NPC_AlertState.Hunt:
                alertValue = Math.Max(alertValue, alert_SuspiciousThresshold + thresholdOffset);
                break;
        }
    }

    private void Awake()
    {
        //Initialising schedule
        for (var i = 0; i < schedule.Count; i++)
        {
            schedule[i] = InitialiseTask(schedule[i]);
        }

        SortSchedule();
        if (bodyCollider)
        {
            originalBodyCenter = bodyCollider.center;
            originalHeight = bodyCollider.height;
        }
    }

    private void Start()
    {
        uiController = UIController.current;
    }

    private void Update()
    {
        if (alertValue > 0f)
        {
            UpdateAlertUI();
        }
    }

    private void FixedUpdate()
    {
        //Not sure if this should be in the behaviour tree or not
        // AlertUpdateBehaviour();
    }

    /// <summary>
    /// For detecting and updating the alert state
    /// </summary>
    /// <returns></returns>
    public NPC_AlertState AlertUpdateBehaviour()
    {
        SmartObject so = FindSS_HackingCamera();
        //will need to change this in to detecting any suspicious item
        if (so)
        {
            if (alertValue < alert_SpottedThresshold)
            {
                UpdateAlertValue(1f);
            }
        }
        else
        {
            if (IsRoaming)
            {
                if (alertValue > 0f)
                {
                    UpdateAlertValue(-1f);
                }
            }
        }

        if (IsRoaming)
        {
            return EvaluateAlertValue();
        }

        return blackboardAlertState;
    }

    public NPC_AlertState EvaluateAlertValue()
    {
        if (alertValue >= alert_SpottedThresshold)
        {
            {
                return NPC_AlertState.Spotted;
            }
        }
        else if (alertValue >= alert_SuspiciousThresshold)
        {
            {
                return NPC_AlertState.Suspicious;
            }
        }

        //TODO: might need to check back on this to find if the world is in alert or not
        switch (blackboardAlertState)
        {
            case NPC_AlertState.Peace:
                return NPC_AlertState.Peace;
                break;
            case NPC_AlertState.Alert:
                return NPC_AlertState.Alert;
                break;
            default:
                return NPC_AlertState.Peace;
                break;
        }

        return blackboardAlertState;
    }

    public CameraObject FindSS_HackingCamera()
    {
        return sensoryController.FindHackingCamera();
    }

    public CameraObject[] FindSS_ActiveCameras()
    {
        return sensoryController.FindActiveCameras();
    }

    public void ResetAlert()
    {
        alertValue = 0;
        UpdateAlertUI();
    }


    public void UpdateAlertValue(float multiplier)
    {
        alertValue = Math.Clamp(alertValue + peaceToAlertSpeed * multiplier * Time.deltaTime, 0f, 1f);
        UpdateAlertUI();
    }

    private void UpdateAlertUI()
    {
        uiController.AlertManager_SetAlert(this, alertValue);
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
        if (taskEvent is {HasObject: true, Position: {magnitude: <= .1f}})
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

    public bool PickUpItem(ItemObject item)
    {
        pickedUpItem = item;
        return item.PickUp(this, GetCurrentTask());
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

    public void AddSensorySource(SensorySource ss)
    {
        sensoryController.AddSS(ss);
    }

    public void RemoveCurrentSensorySource()
    {
        sensoryController.PopCurrentSS();
    }

    public void ShrinkBody()
    {
        if (bodyCollider)
        {
            bodyCollider.center = originalBodyCenter * deathSizeMultiplier;
            bodyCollider.height = originalHeight * deathSizeMultiplier;
        }
    }

    public void ResetBody()
    {
        if (bodyCollider)
        {
            bodyCollider.center = originalBodyCenter;
            bodyCollider.height = originalHeight;
        }
    }
}