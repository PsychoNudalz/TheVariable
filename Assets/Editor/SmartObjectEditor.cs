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
        // GUIUtility.ExitGUI();
    }

    protected virtual void AddHack(HackAbility hack)
    {
        int i = smartObject.Hacks.Length;
        smartObject.AddHack(hack);
        Debug.Log($"Added hack to {smartObject.name}[{i}], now size: {smartObject.Hacks.Length}");
    }
    protected virtual void AddHack<T>(string hackName) where T : new()
    {
        if (GUILayout.Button("Hack: "+hackName))
        {
            var hack = new T();
            AddHack(hack as HackAbility);
            SetDirty();
        }
    }
    
    protected virtual void AddHack_Distraction()
    {
        AddHack<Hack_General_Distraction>("Distraction");

    }
}
#endif