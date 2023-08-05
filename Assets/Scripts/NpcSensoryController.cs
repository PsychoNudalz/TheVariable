using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;


public class NpcSensoryController : MonoBehaviour
{
    [SerializeField]
    private NpcController npcController;
    private SensorySource currentSS = null;

    public SensorySource GetCurrentSS =>currentSS;

    private void Awake()
    {
        if (!npcController)
        {
            npcController = GetComponent<NpcController>();
        }
        
    }

    public void AddSS(SensorySource ss)
    {
        if (currentSS==null)
        {
            currentSS = ss;
        }
        else
        {
            currentSS = SensorySource.Compare(currentSS, ss);
        }
        npcController.SetAlertState(NPC_AlertState.Suspicious);
    }

    public void PopCurrentSS()
    {
        currentSS = null;
    }
}