using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_HackAbilityDisplay : MonoBehaviour
{
    [SerializeField]
    private UI_HackAbilityButton[] buttons;

    private SmartObject currentSO;

    private UI_HackAbilityButton selectedButton;

    // Start is called before the first frame update
    private void Start()
    {
        for (var index = 0; index < buttons.Length; index++)
        {
            var uiHackAbilityButton = buttons[index];
            uiHackAbilityButton.SetDisplay(this,index);
        }
    }

    public void SetObjectToHack(SmartObject so)
    {
        currentSO = so;
        if (!currentSO)
        {
            return;
        }
        for (var index = 0; index < buttons.Length; index++)
        {
            var uiHackAbilityButton = buttons[index];
            if (index < currentSO.Hacks.Length)
            {
                uiHackAbilityButton.SetActive(true,currentSO.Hacks[index].ToString());
            }
            else
            {
                uiHackAbilityButton.SetActive(false);
            }
        }
    }

    public void Hack(int i)
    {
        if (i == -1)
        {
            return;
        }
        currentSO.ActivateHack(i);
    }

    public int UpdateDir(Vector2 dir)
    {
        if (selectedButton)
        {
            selectedButton.OnHover(false);
        }

        if (dir.magnitude < 0.2f)
        {
            return -1;
        }

        dir = dir.normalized;
        selectedButton = buttons[0];
        float dot = Vector2.Dot(selectedButton.GetDotDir(), dir);
        float dotTemp;
        int bIndex = 0;
        for (var index = 0; index < buttons.Length; index++)
        {
            var button = buttons[index];
            if (!button.IsActive)
            {
                break;
            }
            dotTemp = Vector2.Dot(button.GetDotDir(), dir);
            if (dotTemp<0f)
            {
                break;
            }
            if (dotTemp > dot)
            {
                dot = dotTemp;
                selectedButton = button;
                bIndex = index;
            }
        }
        //If the dir is moving in the complete opposite way 
        if (dot < 0f)
        {
            return -1;
        }
        selectedButton.OnHover(true);

        return bIndex;
    }
}