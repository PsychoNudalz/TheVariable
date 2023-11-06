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
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI originalItemName;

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
            if (smartObject is ItemObject itemObject)
            {
                SOInfo.SetActive(true);
                itemName.text = itemObject.CurrentItemName.ToString();
                originalItemName.text = itemObject.OriginalItemName.ToString();
            }
        }
        else
        {
            currentSO = null;
            SOInfo.SetActive(false);
        }
    }
}
