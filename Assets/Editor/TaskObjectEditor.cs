#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TaskObject))]
public class TaskObjectEditor : SmartObjectEditor
{
    public override void GUIContent()
    {
        smartObject = (TaskObject) smartObject;
        AddHack_Distraction();
    }
}
#endif