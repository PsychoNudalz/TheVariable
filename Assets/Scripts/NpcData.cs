using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class NpcData : MonoBehaviour
{
    [FormerlySerializedAs("tasks")]
    [SerializeField]
    [Tooltip("Currently queued task. this is not the schedule")]
    private List<TaskEvent> taskQueue = new List<TaskEvent>();
    [SerializeField]
    [Tooltip("This is the schedule")]
    private List<TaskEvent> schedule = new List<TaskEvent>();

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
            return new TaskEvent();
        }
        else
        {
            return taskQueue[0];
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
                taskQueue[0].TaskObject.Interact();
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
            if (taskQueue[0].HasObject)
            {
                taskQueue[0].TaskObject.Finish(isInterrupt);
            }

            return taskQueue[0].Duration;
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

    public TaskEvent InitialiseTask( TaskEvent taskEvent)
    {
        //setting the position to the task object's interaction point
        if (taskEvent is {HasObject: true, Position: {magnitude: <= .1f}})
        {
            taskEvent.Position = taskEvent.TaskObject.Position;
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
    
}