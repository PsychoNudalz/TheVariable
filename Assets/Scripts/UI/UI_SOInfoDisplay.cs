using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SOInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject SONamePlate;
    [SerializeField]
    private TextMeshProUGUI SOName_Text;

    [Space(5)]
    [Header("Info Display")]
    [SerializeField]
    private GameObject SOInfo;
    private SmartObject currentSO;

    [Header("Items")]
    [SerializeField]
    GameObject itemGroup;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI originalItemName;

    [Header("NPC")]
    [SerializeField]
    GameObject NPCGroup;
    [SerializeField]
    private TextMeshProUGUI NPCState;

    [SerializeField]
    private TextMeshProUGUI taskName;

    [SerializeField]
    private TextMeshProUGUI[] scheduleText;

    // Start is called before the first frame update
    void Start()
    {
        SetSO(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSO(SmartObject smartObject)
    {
        if (smartObject)
        {
            currentSO = smartObject;
            SONamePlate.SetActive(true);
            SOName_Text.text = currentSO.name;
        }
        else
        {
            currentSO = null;
            SONamePlate.SetActive(false);
        }
    }

    public void ShowInfo(SmartObject smartObject)
    {
        if (smartObject)
        {
            currentSO = smartObject;
            SOInfo.SetActive(false);
            itemGroup.SetActive(false);
            NPCGroup.SetActive(false);
            // if (smartObject is ItemObject itemObject)
            // {
            //     SOInfo.SetActive(true);
            //     itemGroup.SetActive(true);
            //     itemName.text = itemObject.CurrentItemName.ToString();
            //     originalItemName.text = itemObject.OriginalItemName.ToString();
            // }

            if (smartObject is NpcObject npcObject)
            {
                SOInfo.SetActive(true);
                NPCGroup.SetActive(true);
                NPCState.text = npcObject.Controller.blackboardAlertState.ToString();
                if (npcObject.Controller.HasCurrentTask())
                {
                    taskName.text = npcObject.Controller.CurrentTask.ToString();
                }
                else
                {
                    taskName.text = "NULL";
                }

                for (var i = 0; i < scheduleText.Length; i++)
                {
                    var text = scheduleText[i];
                    if (i < npcObject.Controller.TaskQueue.Count)
                    {
                        text.gameObject.SetActive(true);
                        text.text = npcObject.Controller.TaskQueue[i].ToString();
                    }
                    else
                    {
                        text.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            currentSO = null;
            SOInfo.SetActive(false);
        }
    }
}
