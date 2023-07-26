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
        AddHack<Hack_General_Distraction>("Distraction");
        AddHack<Hack_Camera_Switch>("Camera Switch");
        // AddHack_Distraction();
        // AddHack_Camera_Switch();

    }



    protected void AddHack_Camera_Switch()
    {
        AddHack<Hack_Camera_Switch>("Camera Switch");

    }
}
#endif