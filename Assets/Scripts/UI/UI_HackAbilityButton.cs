using System;
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

    [SerializeField]
    GameObject lockGO;

    private Vector2 dotDir = default;


    enum ActiveState_Enum
    {
        InActive,
        Active,
        HackFail
    }

    private ActiveState_Enum activeState = ActiveState_Enum.InActive;

    public bool IsActive => activeState == ActiveState_Enum.Active;

    public Vector2 GetDotDir()
    {
        if (dotDir.Equals(default))
        {
            dotDir = (transform.position - display.transform.position).normalized;
        }

        return dotDir;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetActive(bool b, bool canHack = true, string hackName = "No Hack")
    {
        OnHover(false);
        if (b)
        {
            gameObject.SetActive(true);
            if (!canHack)
            {
                lockGO.SetActive(true);
                activeState = ActiveState_Enum.HackFail;
            }
            else
            {
                lockGO.SetActive(false);
                activeState = ActiveState_Enum.Active;
            }
        }
        else
        {
            lockGO.SetActive(false);
            gameObject.SetActive(false);
            activeState = ActiveState_Enum.InActive;
        }

        UpdateButton(hackName);
    }

    public void SetActive(bool b, HackAbility hackAbility)
    {
        SetActive(b, hackAbility.CanHack(), hackAbility.HackName);
    }

    public void SetDisplay(UI_HackAbilityDisplay display, int i)
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