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

    [SerializeField]
    private NpcVisionConeController visionConeController;

    public List<SmartObject> detectedObjects => visionConeController.AllDetectedSmartObjects;
    public List<SmartObject> losObjects => visionConeController.AllLoSSmartObjects;
    


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
        npcController.SetAlertState(NPC_AlertState.Suspicious,1);
    }

    public void PopCurrentSS()
    {
        currentSS = null;
    }

    public bool HasCamera()
    {
        return GetCamera();
    }
    
    public CameraObject GetCamera()
    {
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                return cameraObject;
            }
        }

        return null;
    }
    
    /// <summary>
    /// Find camera that is controlled the player
    /// TODO: one for if the camera is hacking
    /// </summary>
    /// <returns></returns>
    public CameraObject FindPlayerCamera()
    {
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (cameraObject.PlayerControl)
                {
                    return cameraObject;
                }
            }
        }

        return null;
    }
}