using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AlertManager : MonoBehaviour
{
    [SerializeField]
    private UI_Alert uiAlertGo;
    [SerializeField]
    private Dictionary<NpcController, UI_Alert> npcToAlertMap;


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
        npcToAlertMap = new Dictionary<NpcController, UI_Alert>();
        foreach (NpcController npcController in NpcManager.NPCs)
        {
            UI_Alert alert = Instantiate(uiAlertGo,transform).GetComponent<UI_Alert>();
            npcToAlertMap.Add(npcController,alert);
        }
    }

    public void UpdateAlert(NpcController npc, float value)
    {
        npcToAlertMap[npc].SetAlert(npc.AlertPosition,value);
    }
}
