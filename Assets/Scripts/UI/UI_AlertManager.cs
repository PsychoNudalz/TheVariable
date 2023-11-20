using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// New alert manager to update alert
/// </summary>
public class UI_AlertManager : MonoBehaviour
{
    [SerializeField]
    private UI_Alert uiAlertGo;
    [SerializeField]
    private Dictionary<NpcController, UI_Alert_Old> npcToAlertMap;
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
            UI_Alert_Old alertOld = Instantiate(uiAlertGo,transform).GetComponent<UI_Alert_Old>();
            npcToAlertMap.Add(npcController,alertOld);
        }
    }

    public void UpdateAlert(NpcController npc, float value)
    {
        npcToAlertMap[npc].SetAlert(npc.AlertPosition,value);
    }
}
