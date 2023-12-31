using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

[Serializable]
public enum TutorialEnum
{
    Start,
    HackingControls,
    CameraSwitch,
    ClearanceLevel,
    Distraction,
    AlertLevel,
    Investigate,
    SpotAndLockdown,
    LockdownImmunity,
    NPCCamera,
    Zooming,
    Highlight,
    GoldenBytes,
    Minimap,
    HackingControls_2,
    CameraStack,
    Tutorial,
    VIP,
    NPCs,
    Hack_Reset_Alert,
    Hack_Delete

}
//
// [Serializable]
// public class TutorialInstructions1
// {
//     
// }

/// <summary>
/// Handles saving tutorial content
/// Will be called to display tutorial
/// It will call the UI to draw and display the new Tutorial
/// Takes player inputs to navigate tutorial window
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private TutorialInstructions[] tutorials;


    private Queue<TutorialEnum> queue = new Queue<TutorialEnum>();

    private UIController uiController;
    private TutorialInstructions currentTutorial;
    private List<TutorialEnum> alreadyDisplayed = new List<TutorialEnum>();

    public static TutorialManager current;

    public static bool DisableTutorial = false;

    public bool IsActive => uiController.IsTutorialActive;
    private void Awake()
    {
        current = this;
        tutorials = Resources.LoadAll("Tutorial", typeof(TutorialInstructions)).Cast<TutorialInstructions>().ToArray();

        int temp = PlayerPrefs.GetInt("Tutorial");
        if (temp >= 0)
        {
            DisableTutorial = false;
        }
        else
        {
            DisableTutorial = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = UIController.current;
        DisplayTutorial(TutorialEnum.Start);
        Close();
        
        Invoke(nameof(StartingTutorial),3f);
    }

    public void StartingTutorial()
    {
        // Display_FirstTime(TutorialEnum.Start);
        Display_FirstTime(TutorialEnum.HackingControls);
        Display_FirstTime(TutorialEnum.HackingControls_2);
        // Display_FirstTime(TutorialEnum.ClearanceLevel);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnWindow_Next()
    {
        TutorialEnum currentEnum = TutorialEnum.Start;
        if (currentTutorial != null)
        {
            currentEnum = currentTutorial.TutorialEnum;
        }

        if (Enum.IsDefined(typeof(TutorialEnum), currentEnum + 1))
        {
            currentEnum += 1;
        }
        else
        {
            currentEnum = 0;
        }

        DisplayTutorial(currentEnum);
    }

    public void OnWindow_Prev()
    {
        TutorialEnum currentEnum = TutorialEnum.Start;
        if (currentTutorial != null)
        {
            currentEnum = currentTutorial.TutorialEnum;
        }

        if (Enum.IsDefined(typeof(TutorialEnum), currentEnum - 1))
        {
            currentEnum -= 1;
        }

        DisplayTutorial(currentEnum);
    }

    public void OnWindow_Close()
    {
        Close();
    }

    private void Close()
    {
        uiController.Tutorial_Close();
        GameManager.ResetTimeScale();
        GameManager.PauseTimer(false);
        PlayerController.current.LockInput(false);
        if (queue.TryDequeue(out TutorialEnum tutorialEnum))
        {
            DisplayTutorial(tutorialEnum);
        }
        
        SoundManager.current.ResumeSounds();


    }

    public void OnWindow_Tutorial(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            if (currentTutorial != null)
            {
                DisplayTutorial(currentTutorial.TutorialEnum);
            }
            else
            {
                DisplayTutorial(TutorialEnum.HackingControls);
            }
        }
    }

    /// <summary>
    /// Main method to display tutorial
    /// </summary>
    /// <param name="tutorialEnum"></param>
    void DisplayTutorial(TutorialEnum tutorialEnum)
    {
        currentTutorial = null;
        foreach (TutorialInstructions tutorial in tutorials)
        {
            if (tutorial.Equals(tutorialEnum))
            {
                currentTutorial = tutorial;
                break;
            }
        }

        if (currentTutorial == null)
        {
            Debug.LogError($"Can not find tutorial: {tutorialEnum.ToString()}");
            return;
        }

        uiController.Tutorial_Show(currentTutorial.Title, currentTutorial.VideoClip, currentTutorial.Text);
        
        GameManager.StopTime();
        GameManager.PauseTimer(true);
        GameManager.ChangeState(GameManager.GameState.PauseGame);
        
        SoundManager.current.PauseAllSounds();

        PlayerController.current.LockInput(true);
        SoundManager.PlayGlobal(SoundGlobal.Tutorial_On);

    }

    public void PushStack(TutorialEnum tutorialEnum)
    {
        if (!uiController.IsTutorialDisplay)
        {
            DisplayTutorial(tutorialEnum);
        }
        else
        {
            queue.Enqueue(tutorialEnum);
        }

        // if (stack.Count ==1)
        // {
        //     
        // }
        //
    }

    void DisplayTutorial_FirstTime(TutorialEnum tutorialEnum)
    {
        if (DisableTutorial)
        {
            if (!(tutorialEnum is TutorialEnum.Investigate or TutorialEnum.AlertLevel or TutorialEnum.SpotAndLockdown))
            {
                Debug.Log($"Tutorial disabled");
            }

            return;
        }
        if (!alreadyDisplayed.Contains(tutorialEnum))
        {
            alreadyDisplayed.Add(tutorialEnum);
            PushStack(tutorialEnum);
        }
    }

    public static void Display_FirstTime(TutorialEnum tutorialEnum)
    {
        if (current)
        {
            current.DisplayTutorial_FirstTime(tutorialEnum);
        }
    }

    [Command()]
    public static void SetTutorial(bool b)
    {
        DisableTutorial = !b;
        if (DisableTutorial)
        {
            PlayerPrefs.SetInt("Tutorial",-1);
        }
        else
        {
            PlayerPrefs.SetInt("Tutorial",0);
        }
    }
}