﻿using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// Deals with information regarding sensory items
/// </summary>
[System.Serializable]
[CanBeNull]
public abstract class SensorySource
{
    protected SmartObject smartObject = null;
    protected Vector3 position = new Vector3();
    protected float strength = 1f;

    public Vector3 Position => position;

    public float Strength => strength;

    public SmartObject SmartObject => smartObject;

    public SensorySource(Vector3 position, float strength)
    {
        this.position = position;
        this.strength = strength;
    }    public SensorySource(SmartObject so,Vector3 position, float strength)
    {
        smartObject = so;
        this.position = position;
        this.strength = strength;
    }

    public void AdjustStrength(Vector3 target, LayerMask castLayer, float dampenStrength)
    {
        Vector3 diff = (target - position);
        RaycastHit[] hits = Physics.RaycastAll(position, diff.normalized, diff.magnitude, castLayer);
        int foundHits = hits.Length;
        foundHits--;
        if (foundHits > 0)
        {
            strength *= Mathf.Pow(dampenStrength, foundHits);
            Debug.Log($"Dampen from {strength} by {foundHits} times.");

        }
    }

    public static SensorySource Compare(SensorySource ss1, SensorySource ss2)
    {
        if (ss1.strength >= ss2.strength)
        {
            return ss1;
        }

        return ss2;
    }

    public void DecayStrength(float amount)
    {
        strength -= amount;
    }
}

/// <summary>
/// Deals with audio based sensor, area of effect
/// </summary>
[System.Serializable]
[CanBeNull]

public class SensorySource_Audio : SensorySource
{
    public SensorySource_Audio(Vector3 position, float strength) : base(position, strength)
    {
        this.position = position;
        this.strength = strength;
    }
    public SensorySource_Audio(SmartObject so, float strength) : base(so.Position, strength)
    {
        this.position = so.InteractPosition;
        this.strength = strength;
        smartObject = so;
    }

    public SensorySource_Audio(SmartObject so, Vector3 position, float strength) : base(so, position, strength)
    {
        this.position = position;
        this.strength = strength;
        smartObject = so;
    }
}
/// <summary>
/// Deals with visual based sensor
/// Eg,
/// hacking cameras
/// found items on the floor
/// dead bodies
/// </summary>
[System.Serializable]
[CanBeNull]

public class SensorySource_Visual : SensorySource
{
    public SensorySource_Visual(Vector3 position, float strength) : base(position, strength)
    {
        this.position = position;
        this.strength = strength;
    }

    public SensorySource_Visual(SmartObject so, float strength) : base(so.Position, strength)
    {
        this.position = so.InteractPosition;
        this.strength = strength;
        smartObject = so;
    }
}