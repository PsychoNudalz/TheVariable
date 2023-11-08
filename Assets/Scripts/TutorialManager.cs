using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum TutorialEnum
{
    Start,
    HackingControls
}

[Serializable]
public class TutorialInstructions
{
    [SerializeField]
    private TutorialEnum tutorialEnum = TutorialEnum.Start;

    [SerializeField]
    private string title = "";

    [SerializeField]
    private VideoClip videoClip = null;

    [SerializeField]
    [TextAreaAttribute(5,10)]
    private string text = "";

    public TutorialEnum TutorialEnum => tutorialEnum;

    public string Title => title;

    public VideoClip VideoClip => videoClip;

    public string Text => text;

    public TutorialInstructions()
    {
    }

    public TutorialInstructions(TutorialEnum tutorialEnum, string title, VideoClip videoClip, string text)
    {
        this.tutorialEnum = tutorialEnum;
        this.title = title;
        this.videoClip = videoClip;
        this.text = text;
    }

    public override bool Equals(object obj)
    {
        if (obj is TutorialEnum t)
        {
            return t.Equals(tutorialEnum);
        }
        return base.Equals(obj);
    }

    public bool Equals(TutorialEnum t)
    {
        return t.Equals(tutorialEnum);
    }
}

/// <summary>
/// Handles saving tutorial content
/// Will be called to display tutorial
/// It will call the UI to draw and display the new Tutorial
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private TutorialInstructions[] tutorials;

    private UIController uiController;
    private TutorialInstructions currentTutorial;
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = UIController.current;
        DisplayTutorial(TutorialEnum.HackingControls);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTutorial(TutorialEnum tutorialEnum)
    {
        currentTutorial=null;
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
        uiController.Tutorial(currentTutorial.Title,currentTutorial.VideoClip,currentTutorial.Text);
        
    }
}
