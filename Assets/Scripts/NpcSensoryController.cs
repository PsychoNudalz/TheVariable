using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;


/// <summary>
/// This should only be accessed by the NPC controller
/// </summary>
public class NpcSensoryController : MonoBehaviour
{
    [SerializeField]
    private NpcController npcController;

    private SensorySource currentSS = null;

    public SensorySource GetCurrentSS => currentSS;

    [SerializeField]
    private NpcVisionConeController visionConeController;

    public HashSet<SmartObject> detectedObjects => visionConeController.AllDetectedSmartObjects;
    public HashSet<SmartObject> losObjects => visionConeController.AllLoSSmartObjects;


    private void Awake()
    {
        if (!npcController)
        {
            npcController = GetComponent<NpcController>();
        }
    }

    /// <summary>
    /// Add new Sensory source
    /// Return true if the new source is set
    /// </summary>
    /// <param name="ss"></param>
    public bool AddSS(SensorySource ss)
    {
        if (currentSS == null)
        {
            currentSS = ss;
            return true;
        }
        else
        {
            ss = SensorySource.Compare(currentSS, ss);
            if (ss.Equals(currentSS))
            {
                return false;
            }
            else
            {
                currentSS = ss;
                return true;
            }
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

    public CameraController GetCamera()
    {
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                return cameraObject.CameraController;
            }

            if (smartObject is NpcObject npc)
            {
                return npc.Camera;
            }
        }

        return null;
    }

    /// <summary>
    /// Find camera that is hacking
    /// 
    /// </summary>
    /// <returns></returns>
    public CameraController FindHackingCamera()
    {
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (cameraObject.IsHacking)
                {
                    return cameraObject.CameraController;
                }
            }

            if (smartObject is NpcObject npc)
            {
                if (npc.Camera.IsHacking)
                {
                    return npc.Camera;
                }
            }
        }

        return null;
    }

    public CameraController[] FindHackingCameras()
    {
        List<CameraController> cameras = new List<CameraController>();
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (cameraObject.IsHacking)
                {
                    cameras.Add(cameraObject.CameraController);
                }
            }

            if (smartObject is NpcObject npc)
            {
                if (npc.Camera.IsHacking)
                {
                    cameras.Add(npc.Camera);
                }
            }
        }

        return cameras.ToArray();
    }

    /// <summary>
    /// Find cameras that is controlled the player or hacking
    /// 
    /// </summary>
    /// <returns></returns>
    public CameraController[] FindActiveCameras()
    {
        List<CameraController> cameras = new List<CameraController>();
        foreach (SmartObject smartObject in losObjects)
        {
            if (smartObject is CameraObject cameraObject)
            {
                if (!cameraObject.IsLocked && (cameraObject.IsHacking || cameraObject.IsPlayerControl))
                {
                    cameras.Add(cameraObject.CameraController);
                }
            }

            if (smartObject is NpcObject npc)
            {
                if (!npc.Camera.IsLocked && (npc.Camera.IsHacking || npc.Camera.IsPlayerControl))
                {
                    cameras.Add(npc.Camera);
                }
            }
        }

        return cameras.ToArray();
    }
    
    
    public void CleanUpVisionCone()
    {
        visionConeController.CleanUpAllDetectedSO();
    }

}