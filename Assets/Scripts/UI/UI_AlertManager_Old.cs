using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Used the new Alert system",false)]
public class UI_AlertManager_Old : MonoBehaviour
{
    [SerializeField]
    private UI_Alert_Old uiAlertOldGo;
    [SerializeField]
    private Dictionary<NpcController, UI_Alert_Old> npcToAlertMap;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMapping();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialiseMapping()
    {
        npcToAlertMap = new Dictionary<NpcController, UI_Alert_Old>();
        foreach (NpcController npcController in NpcManager.NPCs)
        {
            UI_Alert_Old alertOld = Instantiate(uiAlertOldGo,transform).GetComponent<UI_Alert_Old>();
            npcToAlertMap.Add(npcController,alertOld);
        }
    }

    public void UpdateAlert(NpcController npc, float value)
    {
        npcToAlertMap[npc].SetAlert(npc.AlertPosition,value);
    }
}
