using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    [SerializeField]
    private List<NpcController> npcs;

    public static NpcManager current;
    public static List<NpcController> NPCs => current.npcs;
    private void Awake()
    {
        current = this;
        npcs = new List<NpcController>(FindObjectsOfType<NpcController>(true));
    }


    public static NpcController GetVIP()
    {
        foreach (NpcController npC in NPCs)
        {
            if (npC.IsVip)
            {
                return npC;
            }
        }

        Debug.LogError("Can not find VIP");
        return null;
    }

    public static void SetActive(bool b)
    {
        foreach (NpcController npcController in NPCs)
        {
            npcController.gameObject.SetActive(b);
        }
    }
}
