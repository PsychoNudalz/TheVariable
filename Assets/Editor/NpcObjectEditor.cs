using Hack;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NpcObject))]
public class NpcObjectEditor : SmartObjectEditor
{
    private NpcObject npcObject;

    public override void GUIContent()
    {
        npcObject = (NpcObject) smartObject;
        GUILayout.Label("Hacks");
        AddHack_Distraction();
        AddHack<Hack_NPC_ClearTasks>("Clear Tasks");
        AddHack<Hack_NPC_StraightKill>("Straight KILL");
        AddHack<Hack_NPC_SetPeace>("Set Peace");
    }
}