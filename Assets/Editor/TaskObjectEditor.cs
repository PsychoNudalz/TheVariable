#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TaskObject))]
public class TaskObjectEditor : SmartObjectEditor
{
    private TaskObject taskObject;
    private string taskName = "";
    private int startTime = 0;
    private float duration = 0;

    public override void GUIContent()
    {
        taskObject = (TaskObject)smartObject;
        GUILayout.Label("Hacks");
        AddHack_Distraction();
        GUILayout.Space(5);
        GUILayout.Label("Tasks");
        taskName = EditorGUILayout.TextField("Task name:", taskName);
        startTime = EditorGUILayout.IntField("Start time:", startTime);
        duration = EditorGUILayout.FloatField("Duration:", duration);
        // EditorGUILayout.ObjectField(smartObject, typeof(TaskObject));
        if (GUILayout.Button("Task: Interact"))
        {
            string finalName = string.Concat(taskObject.name, "_Interact_", taskName);

            taskObject.AddAvailableTask(new TaskEvent(finalName, taskObject, startTime, taskObject.InteractPosition,
                duration));
            SetDirty();
        }
        if (GUILayout.Button("Task: Pickup"))
        {
            string finalName = string.Concat(taskObject.name, "_Interact_", taskName);

            taskObject.AddAvailableTask(new TaskEvent(finalName, taskObject, startTime, taskObject.InteractPosition,
                duration));
            SetDirty();
        }
    }
}
#endif