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
    float dataIncreaseAmount = 1f;

    private int currentData = 0;
    private int targetData = 0;
    private int maxData = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Math.Abs(currentData - targetData) > 0.1f)
        {
            UpdateDataUI();
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
        UpdateDataUI();
    }

    public void SetDataMax(int data)
    {
        maxData = data;
        UpdateDataUI();

    }


    void UpdateDataUI()
    {
        if (Math.Abs(currentData - targetData) < dataIncreaseAmount)
        {
            currentData = targetData;
        }
        else
        {
            currentData = Mathf.RoundToInt(Mathf.Lerp(currentData, targetData, dataIncreaseAmount * Time.deltaTime));
        }

        data_Text.text = $"{currentData.ToString()}GB/{maxData.ToString()}GB";
        data_Bar.fillAmount = (float)currentData /(float) maxData;
    }
}