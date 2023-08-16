using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct HackContext
{
    //index:
    //0: Object that is being hacked on
    //1: Object (Camera) where the hack is coming from
    private SmartObject[] smartObjects;

    public SmartObject[] SmartObjects => smartObjects;

    public HackContext(SmartObject[] smartObjects)
    {
        this.smartObjects = smartObjects;
    }
}

/// <summary>
/// Handles all Hacks that involves manipulating Smart Objects
/// Passes context to modify them
///
/// Hacks sound be named Hack_(Object)_(Verb)
/// </summary>
/// 
[Serializable]
public abstract class HackAbility:ScriptableObject
{
    [SerializeField]
    // [HideInInspector]
    protected string HackName;

    [SerializeField]
    private float hackTime = 1f;

    [SerializeField]
    private int hackCost = 1;
    
    protected abstract void AwakeBehaviour();
    protected abstract void StartBehaviour();
    protected abstract void UpdateBehaviour();

    protected bool isActive = false;
    
    [Tooltip("should be left empty by default, this is for keeping track what was the last context for debugging")]
    protected HackContext context = new HackContext(Array.Empty<SmartObject>());

    public string hackName => HackName;

    public float HackTime => hackTime;

    public int hackCost1 => hackCost;

    protected HackAbility()
    {
        HackName = ToString();
    }

    public bool IsActive => isActive;

    /// <summary>
    /// The main hacking procedure. 0: no error, 1: error
    /// </summary>
    /// <param name="hackContext"></param>
    /// <returns>0: no error, 1: error</returns>
    public abstract int Hack(HackContext hackContext);

    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="hackContext"></param>
    // public virtual void SetHack(HackContext hackContext)
    // {
    //     context = hackContext;
    // }
    //
    // public virtual int Hack()
    // {
    //     return Hack(context);
    // }

    public override string ToString()
    {
        string split = GetType().ToString().Substring(5);
        split = split.Replace("_", " ");

        return split;
    }
    
    IEnumerator DelayHack()
    {
        yield return new WaitForSeconds(hackTime);
    }
}