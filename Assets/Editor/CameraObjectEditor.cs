#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CameraObject))]
public class CameraObjectEditor : SmartObjectEditor
{
    public override void OnInspectorGUI()
    {
        smartObject = (SmartObject)target;
        if (GUILayout.Button("Add Camera Switch Hack"))
        {
            Hack_Camera_Switch hackCameraSwitch = new Hack_Camera_Switch();
            AddHack(hackCameraSwitch);
        }
        DrawDefaultInspector();

        SetDirty(smartObject.gameObject);
    }


}
#endif