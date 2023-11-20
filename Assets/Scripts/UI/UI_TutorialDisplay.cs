using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Handles displaying the tutorial to the player UI
/// </summary>
public class UI_TutorialDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private TextMeshProUGUI explainText;

    char prefix_Icon = Convert.ToChar("@");

    char prefix_Button = Convert.ToChar("$");

    public bool IsActive => gameObject.activeSelf;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Display(string title, VideoClip videoClip, string text)
    {
        gameObject.SetActive(true);

        titleText.text = title;

        if (!videoClip)
        {
            videoPlayer.gameObject.SetActive(false);
        }else
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.clip = videoClip;
        }

        string[] splitText = text.Split(" ");
        string reconstructedText = "";
        bool wasWord = false;
        foreach (string word in splitText)
        {
            if (word.Length > 0 && word[0].Equals(prefix_Icon))
            {
                if (wasWord)
                {
                    reconstructedText += "  ";
                }

                reconstructedText += ConvertTextToInputIcon(word);
                wasWord = false;
            }
            else
            {
                reconstructedText += word;
                if (word.Length > 0 && (!word[0].Equals('|')||!word[0].Equals('&')))
                {
                    reconstructedText += " ";
                    wasWord = true;

                }
                else
                {
                    wasWord = false;
                }

            }
        }

        explainText.text = reconstructedText;
    }

    string ConvertTextToInputIcon(string word)
    {
        return UIController.ConvertTextToInputIcon(word, prefix_Button);
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }
}