using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject HUD;
    [SerializeField]
    private UI_HackAbilityDisplay hackAbilityDisplay;

    [SerializeField]
    private UI_AlertManager alertManager;

    [SerializeField]
    private UI_LockOutScreen lockOutScreen;

    [SerializeField]
    private UI_CameraScreen cameraScreen;

    [SerializeField]
    private UI_SOInfoDisplay infoDisplay;

    [SerializeField]
    private UI_TutorialDisplay tutorialDisplay;

    [Header("Smaller Components")]
    [Header("Timer")]
    [SerializeField]
    private GameObject timerDisplay;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [Header("Clearance Level")]
    [SerializeField]
    private TextMeshProUGUI clearanceText;

    [SerializeField]
    private Color[] clearanceColours;
    [Header("Copy Item")]
    [SerializeField]
    private TextMeshProUGUI itemText;

    [Header("Game Over")]
    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private TextMeshProUGUI playTimeText;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    public static UIController current;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        HacksDisplay_SetActive(false);
        SetClearanceText(0);
        gameOverScreen.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HacksDisplay_SetActive(bool b, SmartObject so = null)
    {
        hackAbilityDisplay.gameObject.SetActive(b);
        hackAbilityDisplay.SetObjectToHack(so);
        infoDisplay.ShowInfo(so);
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

    public void SOInfo_Hover(SmartObject so)
    {
        infoDisplay.SetSO(so);
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

    public void StartTimer(float seconds)
    {
        Debug.Log("UI start Timer");
        timerDisplay.SetActive(true);
        UpdateTimer(seconds);
    }

    public void UpdateTimer(float seconds)
    {
        if (seconds <= 0)
        {
            timerText.text = "00:00:00";
            timerText.color = Color.red;
            return;
        }
        var timeString = SecondsToString(seconds);
        timerText.text = timeString;
    }

    private static string SecondsToString(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        string mili = timeSpan.Milliseconds.ToString();
        if (mili.Length >= 2)
        {
            mili = timeSpan.Milliseconds.ToString().Substring(0, 2);
        }
        else
        {
            mili = "0" + mili;
        }

        string sec = timeSpan.Seconds.ToString();
        if (sec.Length == 1)
        {
            sec = "0" + sec;
        }

        string timeString = $"0{timeSpan.Minutes}:{sec}:{mili}";
        return timeString;
    }

    public void ToggleHUD()
    {
        HUD.SetActive(!HUD.activeSelf);

    }
    public void SetHUD(bool b)
    {
        HUD.SetActive(b);
    }

    public void GameOver(float currentTime, float highScore)
    {
        gameOverScreen.SetActive(true);
        playTimeText.text = SecondsToString(currentTime);
        highScoreText.text = SecondsToString(highScore);
    }

    public void Tutorial(string title, VideoClip videoClip, string text)
    {
        tutorialDisplay.Display(title,videoClip,text);
    }
    
}