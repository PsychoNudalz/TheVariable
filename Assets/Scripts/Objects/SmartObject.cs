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
    private float audioDistract_Range = 10f;

    [SerializeField]
    private float audioDistract_Strength = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float audioDistract_Dampen = 1f;

    // [SerializeField]
    LayerMask audioDistraction_LayerMask;

    [SerializeField]
    private ParticleSystem audioDistract_PS;

    [SerializeField]
    private VisualEffect audioDistract_VE;

    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
    public virtual Vector3 ColliderPosition => transform.forward;
    public Vector3 InteractPosition => InteractPointPosition();

    public HackAbility[] Hacks => hacks;

    protected virtual Vector3 InteractPointPosition()
    {
        if (!interactPoint)
        {
            return transform.position;
        }

        return interactPoint.position;
    }

    private void Awake()
    {
        if (!highlightEffect)
        {
            highlightEffect = GetComponent<HighlightEffect>();
        }
        
        audioDistraction_LayerMask = LayerMask.GetMask("Npc","Object");

        //Initialise Hacks
        for (var i = 0; i < hacks.Length; i++)
        {
            HackAbility hackAbility = hacks[i];
            if (hackAbility)
            {
                hackAbility = HackManager.GetHack(hackAbility.hackName);
                hackAbility.Initialise(this);
                hacks[i] = hackAbility;
            }
            
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
        if (audioDistract_PS)
        {
            audioDistract_PS.Play();
        }

        if (audioDistract_VE)
        {
            audioDistract_VE.Play();
        }

        // SensorySource_Audio newSSA = new SensorySource_Audio(InteractPosition, audioDistract_Strength);


        RaycastHit[] castHits = Physics.SphereCastAll(transform.position, audioDistract_Range, Vector3.up, 0,
            audioDistraction_LayerMask);
        Collider collider;
        foreach (RaycastHit hit in castHits)
        {
            collider = hit.collider;
            if (collider.TryGetComponent(out NpcController npcController))
            {
                SensorySource_Audio newSSA = new SensorySource_Audio(InteractPosition, audioDistract_Strength);
                newSSA.AdjustStrength(npcController.transform.position, audioDistraction_LayerMask,
                    audioDistract_Dampen);
                npcController.AddSensorySource(newSSA);
            }
        }
    }
}