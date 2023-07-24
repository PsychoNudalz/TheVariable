using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;

/// <summary>
/// Objects that Both player and npc can interact with and investigate 
/// </summary>
[RequireComponent(typeof(HighlightEffect))]
public abstract class SmartObject : MonoBehaviour
{
    [Header("Smart Object")]
    [SerializeField]
    protected Transform interactPoint;

    private HighlightEffect highlightEffect;

    [Header("Hacks")]
    [SerializeReference]
    private HackAbility[] hacks = Array.Empty<HackAbility>();


    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
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
        highlightEffect = GetComponent<HighlightEffect>();
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
    public virtual int ActivateHack(int i)
    {
        if (CheckHackIndex(i))
        {
            Debug.LogError($"{name} active hack index {i} out of range {hacks.Length}.");
            return 1;
        }

        hacks[i].Hack(new HackContext(new[] { this }));
        return 0;
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

    [ContextMenu("Test hack 0")]
    public void TestHack0()
    {
        ActivateHack(0);
    }
}