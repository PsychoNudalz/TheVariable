using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Objective : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Header("VIP")]
    [SerializeField]
    private GameObject vip_Alive;

    [SerializeField]
    private GameObject extraction;

    [Header("Data")]
    [SerializeField]
    private Image data_Bar;

    [SerializeField]
    private TextMeshProUGUI data_Text;

    [SerializeField]
    float dataIncreaseAmount = 10f;

    private float currentData = 0;
    private float targetData = 0;
    private int maxData = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Math.Abs(currentData - targetData) > 0.1f)
        {
            currentData= Mathf.RoundToInt(UIController.UpdateDelayValueUI(currentData,targetData,maxData,dataIncreaseAmount,data_Text,data_Bar));
        }
    }

    public void KilledVIP()
    {
        StartCoroutine(VIPToExtract());
    }

    IEnumerator VIPToExtract()
    {
        animator.SetTrigger("Play");
        yield return new WaitForSeconds(.1f);
        vip_Alive.SetActive(false);
        extraction.SetActive(true);
    }

    public void SetData(int data)
    {
        targetData = data;
        currentData= Mathf.RoundToInt(UIController.UpdateDelayValueUI(currentData,targetData,maxData,dataIncreaseAmount,data_Text,data_Bar));

    }

    public void SetDataMax(int data)
    {
        maxData = data;
        currentData= Mathf.RoundToInt(UIController.UpdateDelayValueUI(currentData,targetData,maxData,dataIncreaseAmount,data_Text,data_Bar));


    }


    
}