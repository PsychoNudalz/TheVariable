#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraObject))]
public class CameraObjectEditor : SmartObjectEditor
{

    public override void GUIContent()
    {
        smartObject = (CameraObject)smartObject;
        AddHack_Distraction();
        AddHack_Camera_Switch();

    }

    protected void AddHack_Camera_Switch()
    {
        if (GUILayout.Button("Add Camera Switch Hack"))
        {
            Hack_Camera_Switch hack = new Hack_Camera_Switch();
            AddHack(hack);
            SetDirty();
        }
    }
}
#endif