#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// [CustomEditor(typeof(CameraObject))]
public class CameraObjectEditor : SmartObjectEditor
{
    public override void OnInspectorGUI()
    {
        smartObject = (SmartObject)target;
        if (GUILayout.Button("Add Distraction Hack"))
        {
            Hack_General_Distraction hack = new Hack_General_Distraction();
            AddHack(hack);
            SetDirty(smartObject.gameObject);

        }
        if (GUILayout.Button("Add Camera Switch Hack"))
        {
            Hack_Camera_Switch hack = new Hack_Camera_Switch();
            AddHack(hack);
            SetDirty(smartObject.gameObject);

        }

        GUILayout.Space(10f);
        DrawDefaultInspector();

    }


}
#endif