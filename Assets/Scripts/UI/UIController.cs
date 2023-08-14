using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UI_HackAbilityDisplay hackAbilityDisplay;

    [SerializeField]
    private UI_AlertManager alertManager;
    public static UIController current;
    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        HacksDisplay_SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HacksDisplay_SetActive(bool b, SmartObject so = null)
    {
        hackAbilityDisplay.gameObject.SetActive(b);
        hackAbilityDisplay.SetObjectToHack(so);
    }

    public int HacksDisplay_UpdateDir(Vector2 dir)
    {
        return hackAbilityDisplay.UpdateDir(dir);
    }

    public void HacksDisplay_SelectHack(Vector2 dir)
    {
        hackAbilityDisplay.Hack(hackAbilityDisplay.UpdateDir(dir));
    }

    public void AlertManager_SetAlert(NpcController npc, float value)
    {
        alertManager.UpdateAlert(npc,value);
    }
}