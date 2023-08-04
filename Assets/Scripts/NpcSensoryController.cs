using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SensorySource
{
    protected Vector3 positsion;
    protected float strength;

    public Vector3 Position => positsion;

    public float Strength => strength;

    public SensorySource(Vector3 positsion, float strength)
    {
        this.positsion = positsion;
        this.strength = strength;
    }
}

public class SensorySource_Audio : SensorySource
{
    public SensorySource_Audio(Vector3 positsion, float strength) : base(positsion, strength)
    {
        this.positsion = positsion;
        this.strength = strength;
    }
}

public class SensorySource_Visual : SensorySource
{
    public SensorySource_Visual(Vector3 positsion, float strength) : base(positsion, strength)
    {
        this.positsion = positsion;
        this.strength = strength;
    }
}

public class NpcSensoryController : MonoBehaviour
{
    [SerializeField]
    private NpcController npcController;
    private List<SensorySource> sensorySources = new List<SensorySource>();

    public SensorySource GetCurrentSS =>sensorySources[0];

    private void Awake()
    {
        if (!npcController)
        {
            npcController = GetComponent<NpcController>();
        }
        
    }

    public void AddSSA(SensorySource_Audio ssa)
    {
        if (sensorySources.Count == 0)
        {
            sensorySources.Add(ssa);
        }
        else
        {
            sensorySources[0] = ssa;
        }

        npcController.SetAlertState(NPC_AlertState.Suspicious);
    }
}