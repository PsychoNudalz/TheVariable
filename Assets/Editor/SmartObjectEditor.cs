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
        DrawDefaultInspector();
        smartObject = (SmartObject)target;
    }

    protected virtual void AddHack(HackAbility hack)
    {
        int i = smartObject.Hacks.Length;
        smartObject.AddHack(hack);
        Debug.Log($"Added hack to {smartObject.name}[{i}], now size: {smartObject.Hacks.Length}");
    }
}
#endif