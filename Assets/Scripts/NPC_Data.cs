using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class NPC_Data : MonoBehaviour
{
    [SerializeField]
    private List<TaskEvent> tasks = new List<TaskEvent>();

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
        return tasks.Count > 0;
    }

    public void AddTask(TaskEvent t)
    {
        tasks.Add(t);
    }

    public void RemoveTask(TaskEvent t)
    {
        tasks.Remove(t);
    }

    public void RemoveTask()
    {
        if (tasks.Count == 0)
        {
            return;
        }
        tasks.RemoveAt(0);
    }

    public TaskEvent GetCurrentTask()
    {
        if (!HasTasks())
        {
            return new TaskEvent();
        }
        else
        {
            return tasks[0];
        }
    }

    [ContextMenu("Add test task")]
    public void AddTestTask()
    {
        AddTask(new TaskEvent("Eat Food", 0, new Vector3(10, 0, 10), Random.Range(1f, 4f)));
    }
}