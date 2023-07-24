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
            uiHackAbilityButton.SetActive(false);
        }
    }

    public void Hack(int i)
    {
        currentSO.ActivateHack(i);
    }
}