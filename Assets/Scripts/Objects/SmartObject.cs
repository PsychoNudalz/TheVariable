using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Objects that Both player and npc can interact with and investigate 
/// </summary>
[RequireComponent(typeof(HighlightEffect))]
public abstract class SmartObject : MonoBehaviour
{
    [Header("Smart Object")]
    [SerializeField]
    protected Transform interactPoint;

    [SerializeField]
    private HighlightEffect highlightEffect;

    [Header("Hacks")]
    [SerializeReference]
    private HackAbility[] hacks = Array.Empty<HackAbility>();

    [Header("Audio Distract")]
    [SerializeField]
    private SO_AudioDistraction audioDistract;
    
    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
    public virtual Vector3 ColliderPosition => transform.position;
    public Transform InteractTransform => InteractPointTransform();
    public Vector3 InteractPosition => InteractPointPosition();
    public Quaternion InteractRotation => InteractPointTransform().rotation;

    public HackAbility[] Hacks => hacks;
    [Header("Debug")]
    [SerializeField]
    private bool Enable_Debug = true;


    private void OnDrawGizmos()
    {
        if (Enable_Debug)
        {
            Gizmos.color = new Color(.5f,.5f,.5f,.5f);
            if (interactPoint)
            {
                Gizmos.DrawCube(InteractPosition+new Vector3(0f,.1f,0f),new Vector3(.25f,.25f,.25f));
            }
        }
    }
    protected virtual Vector3 InteractPointPosition()
    {
        if (!interactPoint)
        {
            return transform.position;
        }

        return interactPoint.position;
    }
    protected virtual Transform InteractPointTransform()
    {
        if (!interactPoint)
        {
            return transform;
        }

        return interactPoint;
    }

    private void Awake()
    {
        if (!highlightEffect)
        {
            highlightEffect = GetComponent<HighlightEffect>();
        }
        

        //Initialise Hacks
        // for (var i = 0; i < hacks.Length; i++)
        // {
        //     HackAbility hackAbility = hacks[i];
        //     if (hackAbility)
        //     {
        //         hackAbility = HackManager.GetHack(hackAbility.HackName);
        //         hackAbility.Initialise(this);
        //         hacks[i] = hackAbility;
        //     }
        //     
        // }

        foreach (HackAbility hackAbility in hacks)
        {
            hackAbility.Initialise(this);
        }

        if (!audioDistract)
        {
            audioDistract = GetComponent<SO_AudioDistraction>();
        }

        AwakeBehaviour();
    }

    private void Start()
    {
        StartBehaviour();
    }

    private void Update()
    {
        UpdateBehaviour();
    }

    protected abstract void AwakeBehaviour();

    // Start is called before the first frame update
    protected abstract void StartBehaviour();

    // Update is called once per frame
    protected abstract void UpdateBehaviour();
    public abstract void Interact(NpcController npc);

    public virtual void OnSelect_Enter()
    {
        highlightEffect.highlighted = true;
    }

    public virtual void OnSelect_Exit()
    {
        highlightEffect.highlighted = false;
    }

    public void AddHack(HackAbility hackAbility)
    {
        List<HackAbility> temp = new List<HackAbility>(hacks);
        temp.Add(hackAbility);
        hacks = (temp.ToArray());
    }

    /// <summary>
    /// For activating the hack at index i
    /// Might need to override this if the hack needs to pass a special Hack Context
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public virtual int ActivateHack(int i, HackContext_Enum[] hackContextEnum = default)
    {
        if (CheckHackIndex(i))
        {
            Debug.LogError($"{name} active hack index {i} out of range {hacks.Length}.");
            return 1;
        }

        hacks[i].Hack(new HackContext(new[] {this}, hackContextEnum));
        return 0;
    }

    public virtual int ActivateHack<T>(HackContext_Enum[] hackContextEnum = default)
    {
        for (var index = 0; index < hacks.Length; index++)
        {
            HackAbility hackAbility = hacks[index];
            if (typeof(T) == hackAbility.GetType())
            {
                return ActivateHack(index, hackContextEnum);
            }
        }

        return 1;
    }

    /// <summary>
    /// Check if index is valid
    /// true if it fails
    /// </summary>
    /// <param name="i"></param>
    /// <returns>true if it fails</returns>
    protected bool CheckHackIndex(int i)
    {
        return i >= hacks.Length || i < 0;
    }

    public int GetHackIndex<T>()
    {
        for (var index = 0; index < hacks.Length; index++)
        {
            HackAbility hackAbility = hacks[index];
            if (typeof(T) == hackAbility.GetType())
            {
                return index;
            }
        }

        return -1;
    }


    [ContextMenu("Test hack 0")]
    public void TestHack0()
    {
        ActivateHack(0);
    }

    public virtual void CreateAudioDistraction()
    {

        if (audioDistract)
        {
            audioDistract.CreateAudioDistraction(this,transform.position);
        }
        else
        {
            Debug.Log($"{name}: Does not allow audio distraction");
        }
       
    }
}