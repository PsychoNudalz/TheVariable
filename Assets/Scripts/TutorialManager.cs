using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

[Serializable]
public enum TutorialEnum
{
    Start,
    HackingControls,
    CameraSwitch
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

    private void Awake()
    {
        current = this;
        tutorials = Resources.LoadAll("Tutorial", typeof(TutorialInstructions)).Cast<TutorialInstructions>().ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = UIController.current;
        DisplayTutorial(TutorialEnum.Start);
        Close();
        Display_FirstTime(TutorialEnum.Start);
        Display_FirstTime(TutorialEnum.HackingControls);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnWindow_Next(InputValue inputValue)
    {
        if (inputValue.isPressed)
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

            DisplayTutorial(currentEnum);
        }
    }

    public void OnWindow_Prev(InputValue inputValue)
    {
        if (inputValue.isPressed)
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
    }

    public void OnWindow_Close(InputValue inputValue)
    {
        Close();
    }

    private void Close()
    {
        uiController.Tutorial_Close();
        if (queue.TryDequeue(out TutorialEnum tutorialEnum))
        {
            DisplayTutorial(tutorialEnum);
        }
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

    public void DisplayTutorial_FirstTime(TutorialEnum tutorialEnum)
    {
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
}