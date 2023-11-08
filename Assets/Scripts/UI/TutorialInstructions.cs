using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(menuName = "Tutorial Instructions")]
[Serializable]
public class TutorialInstructions : ScriptableObject
{
    [SerializeField]
    private TutorialEnum tutorialEnum = TutorialEnum.Start;

    [SerializeField]
    private string title = "";

    [SerializeField]
    private VideoClip videoClip = null;

    [SerializeField]
    [TextAreaAttribute(5, 10)]
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