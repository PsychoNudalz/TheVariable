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
        videoPlayer.clip = videoClip;

        string[] splitText = text.Split(" ");
        string reconstructedText = "";
        foreach (string word in splitText)
        {
            if (word.Length > 0 && word[0].Equals(prefix_Icon))
            {
                reconstructedText += ConvertTextToIcon(word) + " ";
            }
            else
            {
                reconstructedText += word + " ";
            }
        }

        explainText.text = reconstructedText;
    }

    string ConvertTextToIcon(string word)
    {
        string plateform = "";
        string[] wordSplit = word.Split(prefix_Button);
        if (wordSplit.Length < 2)
        {
            Debug.LogError($"Failed to convert: {word}");
            return word;
        }

        //Selecting platform
        if (wordSplit[0].Equals("@S"))
        {
            //Steam Deck
            plateform = "SteamDeck_Controls";
        }
        else if (wordSplit[0].Equals("@P"))
        {
            //Play Station
            plateform = "PS_Controls";
        }
        else
        {
            //PC
            plateform = "Keyboard_Controls";
        }

        string control = wordSplit[1];

        return string.Concat("<sprite=\"", plateform, "\" name=\"", control, "\">");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}