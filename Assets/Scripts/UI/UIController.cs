using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UI_HackAbilityDisplay hackAbilityDisplay;

    [SerializeField]
    private UI_AlertManager alertManager;

    [SerializeField]
    private UI_LockOutScreen lockOutScreen;

    [SerializeField]
    private UI_CameraScreen cameraScreen;

    [Header("Smaller Components")]
    [Header("Clearance Level")]
    [SerializeField]
    private TextMeshProUGUI clearanceText;

    [SerializeField]
    private Color[] clearanceColours;
    [Header("Copy Item")]
    [SerializeField]
    private TextMeshProUGUI itemText;

    public static UIController current;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        HacksDisplay_SetActive(false);
        SetClearanceText(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HacksDisplay_SetActive(bool b, SmartObject so = null)
    {
        hackAbilityDisplay.gameObject.SetActive(b);
        hackAbilityDisplay.SetObjectToHack(so);
    }

    public int HacksDisplay_UpdateDir(Vector2 dir)
    {
        return hackAbilityDisplay.UpdateDir(dir);
    }

    /// <summary>
    ///     Starting the hack
    /// </summary>
    /// <param name="dir"></param>
    public int HacksDisplay_SelectHack(Vector2 dir)
    {
        int hackIndex = hackAbilityDisplay.UpdateDir(dir);
        // hack = hackAbilityDisplay.GetHack(hackIndex);
        // hackAbilityDisplay.Hack(hackIndex);
        return hackIndex;
    }

    // public int HacksDisplay_SelectHack(Vector2 dir, out HackAbility hack)
    // {
    //     int hackIndex = hackAbilityDisplay.UpdateDir(dir);
    //     hack = hackAbilityDisplay.GetHack(hackIndex);
    //     // hackAbilityDisplay.Hack(hackIndex);
    //     return hackIndex;
    // }

    public void AlertManager_SetAlert(NpcController npc, float value)
    {
        alertManager.UpdateAlert(npc, value);
    }

    public void LockoutScreen_SetActive(bool b, CameraController cameraController = null)
    {
        lockOutScreen.SetActive(b, cameraController);
    }

    public void CameraScreen_Play(CameraInvestigationMode cameraInvestigationMode)
    {
        cameraScreen.PlayAnimation(cameraInvestigationMode);
    }

    public void SetClearanceText(int i)
    {
        clearanceText.text = i.ToString();
        try
        {
            clearanceText.color = clearanceColours[i];
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError("Clearance colour out of range");
            clearanceText.color = Color.white;
        }
    }

    public void SetItem(ItemName s)
    {
        itemText.text = s.ToString();
    }
}