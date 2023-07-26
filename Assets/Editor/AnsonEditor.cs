#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class AnsonEditor : Editor
{
    public virtual void SetDirty(GameObject[] gos)
    {
        foreach (GameObject gameObject in gos)
        {
            EditorUtility.SetDirty(gameObject);
        }
    }

    public virtual void SetDirty(GameObject go)
    {
        EditorUtility.SetDirty(go);
    }

    public virtual void SetDirty()
    {
        EditorUtility.SetDirty(((MonoBehaviour)target).gameObject);
        EditorUtility.SetDirty(((MonoBehaviour)target));

    }

    public virtual void GUIContent()
    {
        
    }
}
#endif
