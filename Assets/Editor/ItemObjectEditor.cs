#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ItemObject))]
public class ItemObjectEditor : SmartObjectEditor
{
    protected ItemObject itemObject;

    public override void GUIContent()
    {
        itemObject = (ItemObject)smartObject;
        AddHack<Hack_General_Distraction>("Distraction");

    }

   
}
#endif