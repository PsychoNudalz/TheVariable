#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// [CustomEditor(typeof(SmartObject))]
public class SmartObjectEditor : AnsonEditor
{
    protected SmartObject smartObject;

    public override void OnInspectorGUI()
    {
        smartObject = (SmartObject)target;
        GUIContent();
        GUILayout.Space(10f);
        DrawDefaultInspector();
        
    }

    protected virtual void AddHack(HackAbility hack)
    {
        int i = smartObject.Hacks.Length;
        smartObject.AddHack(hack);
        Debug.Log($"Added hack to {smartObject.name}[{i}], now size: {smartObject.Hacks.Length}");
    }
    
    protected virtual void AddHack_Distraction()
    {
        if (GUILayout.Button("Add Distraction Hack"))
        {
            Hack_General_Distraction hack = new Hack_General_Distraction();
            AddHack(hack);
            SetDirty();
        }
    }
}
#endif