using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HackAbilityButton : MonoBehaviour
{
    [SerializeField]
    private int index;

    [Header("Colour")]
    [SerializeField]
    Color baseColour = Color.grey;

    [SerializeField]
    Color selectColour = Color.white;

    [Space(5)]
    [SerializeField]
    private UI_HackAbilityDisplay display;

    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetActive(bool b, string hackName = "No Hack")
    {
        OnHover(false);
        if (b)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        UpdateButton(hackName);

    }

    public void SetDisplay(UI_HackAbilityDisplay display,int i)
    {
        this.display = display;
    }

    public void UpdateButton(string s)
    {
        text.SetText(s);
    }

    public void OnHover(bool enter = true)
    {
        if (enter)
        {
            buttonImage.color = selectColour;
        }
        else
        {
            buttonImage.color = baseColour;
        }
    }

    public void ActivateHack()
    {
        display.Hack(index);
    }
}