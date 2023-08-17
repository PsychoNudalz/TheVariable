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

    public void AddSS(SensorySource ss,bool setToSus = true)
    {
        if (currentSS==null)
        {
            currentSS = ss;
        }
        else
        {
            currentSS = SensorySource.Compare(currentSS, ss);
        }

        if (setToSus)
        {
            npcController.SetMinAlertValue(NPC_AlertState.Suspicious);
        }
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
    /// Find camera that is hacking
    /// 
    /// </summary>
    /// <returns></returns>
    public CameraObject FindHackingCamera()
    {
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (cameraObject.IsHacking)
                {
                    return cameraObject;
                }
            }
        }

        return null;
    }
    
    /// <summary>
    /// Find camera that is controlled the player or hacking
    /// 
    /// </summary>
    /// <returns></returns>
    public CameraObject[] FindActiveCameras()
    {
        List<CameraObject> cameras = new List<CameraObject>();
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (cameraObject.IsHacking||cameraObject.IsPlayerControl)
                {
                    cameras.Add(cameraObject);
                }
            }
        }

        return cameras.ToArray();
    }
    

}