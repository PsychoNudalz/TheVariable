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
    
    
    [Header("Vision Cone Cleanup")]
    [SerializeField]
    float cleanUpTime = 5f;
    [SerializeField]
    float cleanUpTimeNow = 0;

    [SerializeField]
    private int npcIndex = 0;
    private void Awake()
    {
        current = this;
        npcs = new List<NpcController>(FindObjectsOfType<NpcController>(true));
    }

    private void FixedUpdate()
    {
        cleanUpTimeNow += Time.deltaTime;
        if (cleanUpTimeNow > cleanUpTime)
        {
            cleanUpTimeNow = 0;
            npcs[npcIndex].CleanUpVisionCone();
            npcIndex = (npcIndex + 1) % npcs.Count;
        }
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
