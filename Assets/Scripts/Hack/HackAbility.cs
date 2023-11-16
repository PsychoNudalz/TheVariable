using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Preset settings to add to hacking context and ability
/// </summary>
[Serializable]
public enum HackContext_Enum
{
    None,
    Camera_notPushToStack
}

[Serializable]
public struct HackContext
{
    //index:
    //0: Object that is being hacked on
    //1: Object (Camera) where the hack is coming from
    private SmartObject[] smartObjects;

    private HackContext_Enum[] hackContextEnum;

    public SmartObject[] SmartObjects => smartObjects;

    public HackContext_Enum[] HackContextEnum => hackContextEnum;


    public HackContext(SmartObject[] smartObjects)
    {
        this.smartObjects = smartObjects;
        hackContextEnum = new HackContext_Enum[] {HackContext_Enum.None};
    }

    public HackContext(SmartObject[] smartObjects, HackContext_Enum[] hackContextEnum)
    {
        this.smartObjects = smartObjects;
        this.hackContextEnum = hackContextEnum;
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
public abstract class HackAbility : ScriptableObject
{
    // [SerializeField]
    // [HideInInspector]
    // protected string HackName;

    [SerializeField]
    protected float hackTime;

    [SerializeField]
    private int hackClearance;
    [SerializeField]
    protected int hackCost;

    protected bool showHack = true;

    public bool ShowHack => showHack;

    protected SmartObject smartObject;

    public virtual void Initialise(SmartObject smartObject)
    {
        this.smartObject = smartObject;
    }

    protected abstract void AwakeBehaviour();
    protected abstract void StartBehaviour();
    protected abstract void UpdateBehaviour();

    protected bool isActive = false;

    [Tooltip("should be left empty by default, this is for keeping track what was the last context for debugging")]
    protected HackContext context = new HackContext(Array.Empty<SmartObject>());

    public string HackName => ToString();

    public float HackTime => hackTime;

    public int HackClearance => hackClearance;

    public int HackCost => hackCost;

    public bool IsHackable => CanHack();

    public bool IsActive => isActive;

    /// <summary>
    /// The main hacking procedure. 0: no error, 1: error, -1: failed can hack
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
        string split = "";
        try
        {
            split = name.Substring(5);
        }
        catch (ArgumentOutOfRangeException e)
        {
            split = GetType().ToString().Substring(5);
        }

        split = split.Replace("_", " ");

        return split;
    }

    public virtual bool CanHack()
    {
        return PlayerController.current.ClearanceLevel>=HackClearance;
    }
}