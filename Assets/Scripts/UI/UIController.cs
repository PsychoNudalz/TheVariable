using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIController : MonoBehaviour
{
    [Serializable]
    class RoomSpritePair
    {
        [SerializeField]
        private RoomLabel room;

        [SerializeField]
        private Sprite iconSprite;

        public RoomLabel Room => room;

        public Sprite IconSprite => iconSprite;

        public RoomSpritePair(RoomLabel room, Sprite iconSprite)
        {
            this.room = room;
            this.iconSprite = iconSprite;
        }

        public bool Equals(RoomLabel r)
        {
            return r.Equals(room);
        }

        public override bool Equals(object obj)
        {
            if (obj is RoomLabel r)
            {
                return r.Equals(room);
            }

            return base.Equals(obj);
        }
    }

    [SerializeField]
    private RoomSpritePair[] roomSpritePairs;

    [Header("Components")]
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

    [SerializeField]
    private UI_CameraInfo cameraInfo;

    [SerializeField]
    private UI_Minimap minimap;

    [SerializeField]
    private UI_ClearanceLevel clearanceLevel;

    [SerializeField]
    private UI_Objective objective;

    [SerializeField]
    private UI_CameraStack cameraStack;

    [SerializeField]
    private UI_GameFinish gameFinish;

    [Header("Smaller Components")]
    [Header("Timer")]
    [SerializeField]
    private GameObject timerDisplay;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Copy Item")]
    [SerializeField]
    private TextMeshProUGUI itemText;

    [Header("Hacking")]
    [SerializeField]
    private GameObject speedUpDisplay;

    [SerializeField]
    private Animator speedUpDisplay_animator;

    public bool IsTutorialActive => tutorialDisplay.IsActive;


    public static UIController current;

    public bool IsTutorialDisplay => tutorialDisplay.gameObject.activeSelf;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        HacksDisplay_SetActive(false);
        // SetClearanceText(0);
        gameFinish.SetActive(false);
        HackSpeedUp_SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    [ContextMenu("RegenerateRoomSpriteList")]
    public void RegenerateRoomSpriteList()
    {
        List<RoomSpritePair> list = new List<RoomSpritePair>(roomSpritePairs);
        foreach (RoomLabel label in Enum.GetValues(typeof(RoomLabel)))
        {
            bool foundPair = false;
            foreach (RoomSpritePair pair in list)
            {
                if (pair.Equals(label))
                {
                    foundPair = true;
                    break;
                }
            }

            if (!foundPair)
            {
                list.Add(new RoomSpritePair(label, null));
            }
        }

        roomSpritePairs = list.ToArray();
    }

    public Sprite GetSprite(RoomLabel roomLabel)
    {
        foreach (RoomSpritePair roomSpritePair in roomSpritePairs)
        {
            if (roomSpritePair.Equals(roomLabel))
            {
                return roomSpritePair.IconSprite;
            }
        }

        Debug.LogError($"Failed to find map sprite: {roomLabel.ToString()}");
        return null;
    }

    public void HacksDisplay_SetActive(bool b, SmartObject so = null)
    {
        hackAbilityDisplay.SetActive(b);
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
        clearanceLevel.SetClearanceText(i);
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

    public static string SecondsToString(float seconds)
    {
        if (seconds > Mathf.Infinity - 1f)
        {
            return "NULL";
        }
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

    public void GameOver(float currentTime, float fastestTime,float currentScore,float highScore)
    {
        gameFinish.GameOver(currentTime,fastestTime,currentScore, highScore);
    }

    public void GameWin(float currentTime, float fastestTime,float currentScore,float highScore)
    {
        gameFinish.GameWin(currentTime,fastestTime,currentScore, highScore);
    }

    public void Tutorial_Show(string title, VideoClip videoClip, string text)
    {
        tutorialDisplay.Display(title, videoClip, text);
    }

    public void Tutorial_Close()
    {
        tutorialDisplay.Close();
    }

    public void SetCamera(CameraController cameraController)
    {
        cameraInfo.SetCamera(cameraController);
    }

    public void Minimap_Active(RoomLabel roomLabel)
    {
        minimap.RoomPin_Active(roomLabel);
    }

    public void Objective_KilledVIP()
    {
        objective.KilledVIP();
    }

    public void Objective_ShowExtraction()
    {
        minimap.ShowExtraction();
    }

    public void Objective_SetDataMax(int data)
    {
        objective.SetDataMax(data);
    }

    public void Objective_SetData(int data)
    {
        objective.SetData(data);
    }

    public void CameraStack_AddStack(CameraController[] cameraControllers, int i)
    {
        cameraStack.UpdateStack(cameraControllers, i);
    }

    public void CameraStack_SetIndex(int i)
    {
        cameraStack.SetIndex(i);
    }

    public void HackSpeedUp_SetActive(bool b)
    {
        speedUpDisplay.SetActive(b);
    }

    public static float UpdateDelayValueUI(float currentData, float targetData, float maxData, float dataIncreaseAmount,
        TextMeshProUGUI data_Text = null, Image data_Bar = null)
    {
        if (Math.Abs(currentData - targetData) < dataIncreaseAmount)
        {
            currentData = targetData;
        }
        else
        {
            currentData = Mathf.Lerp(currentData, targetData, dataIncreaseAmount * Time.deltaTime);
        }

        if (data_Text)
        {
            data_Text.text = $"{currentData:0}GB/{maxData:0}GB";
        }

        if (data_Bar)
        {
            data_Bar.fillAmount = currentData / maxData;
        }

        return currentData;
    }
    public static string ConvertTextToInputIcon(string word, char prefix_Button)
    {
        string platform = "";
        bool hasComma = false;
        string[] wordSplit = word.Split(prefix_Button);
        if (word.Contains("."))
        {
            word.Remove(word.Length - 1);
            hasComma = true;
        }

        if (wordSplit.Length < 2)
        {
            Debug.LogError($"Failed to convert: {word}");
            return word;
        }

        //Selecting platform
        if (wordSplit[0].Equals("@S"))
        {
            //Steam Deck
            platform = "SteamDeck_Controls";
        }
        else if (wordSplit[0].Equals("@P"))
        {
            //Play Station
            platform = "PS_Controls";
        }
        else
        {
            //PC
            platform = "Keyboard_Controls";
        }

        string control = wordSplit[1];

        string returnString = string.Concat("<sprite=\"", platform, "\" name=\"", control, "\">");
        if (hasComma)
        {
            returnString += ".";
        }

        return returnString;
    }
}