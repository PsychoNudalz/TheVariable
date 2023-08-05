﻿using UnityEngine;

[System.Serializable]
public class SensorySource
{
    protected Vector3 position;
    protected float strength;

    public Vector3 Position => position;

    public float Strength => strength;

    public SensorySource(Vector3 position, float strength)
    {
        this.position = position;
        this.strength = strength;
    }

    public void AdjustStrength(Vector3 target, LayerMask castLayer, float dampenStrength)
    {
        Vector3 diff = (target - position);
        RaycastHit[] hits = Physics.RaycastAll(position, diff.normalized, diff.magnitude, castLayer);
        int foundHits = hits.Length;
        foundHits--;
        Debug.Log($"Dampen from {strength} by {foundHits} times.");
        if (foundHits > 0)
        {
            strength *= Mathf.Pow(dampenStrength, foundHits);
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
}

[System.Serializable]
public class SensorySource_Audio : SensorySource
{
    public SensorySource_Audio(Vector3 position, float strength) : base(position, strength)
    {
        this.position = position;
        this.strength = strength;
    }
}

[System.Serializable]
public class SensorySource_Visual : SensorySource
{
    public SensorySource_Visual(Vector3 position, float strength) : base(position, strength)
    {
        this.position = position;
        this.strength = strength;
    }
}