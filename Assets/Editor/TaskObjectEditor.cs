#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEditor;
using UnityEngine;
// using System.Runtime.InteropServices;


[CustomEditor(typeof(TaskSmartObject))]
public class TaskObjectEditor : SmartObjectEditor
{
    private TaskSmartObject taskSmartObject;
    private string taskName = "";
    private int startTime = 0;
    private float duration = 0;

    public override void GUIContent()
    {
        taskSmartObject = (TaskSmartObject)smartObject;
        GUILayout.Label("Hacks");
        AddHack_Distraction();
        GUILayout.Space(5);
        GUILayout.Label("Tasks");
        taskName = EditorGUILayout.TextField("Task name:", taskName);
        startTime = EditorGUILayout.IntField("Start time:", startTime);
        duration = EditorGUILayout.FloatField("Duration:", duration);
        // EditorGUILayout.ObjectField(smartObject, typeof(TaskObject));
        if (GUILayout.Button("Create and Add Task"))
        {
            string finalName = string.Concat(taskSmartObject.name, "_", taskName);

            taskSmartObject.AddAvailableTask(new TaskEvent());
            SetDirty();
        }

        // if (GUILayout.Button("Test TaskEvent byte size"))
        // {
        //     Debug.LogWarning("Tasks size: "+Marshal.SizeOf(taskObject.AvailableTasks));
        // }

    }
}
#endif