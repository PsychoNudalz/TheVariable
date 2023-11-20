using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

/// <summary>
/// Handles effects
/// </summary>
public class TaskObjectVisualController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField]
    private UnityEvent onInteractEvent;
    [SerializeField]
    private UnityEvent onDepositEvent;
    [SerializeField]
    private UnityEvent onFinishEvent;
    [SerializeField]
    private UnityEvent onInterruptEvent;

    // [SerializeField]
    // private ParticleSystem[] particleSystems;
    // [SerializeField]
    // private VisualEffect[] visualEffects;
    //
    // private void Awake()
    // {
    //     foreach (ParticleSystem system in particleSystems)
    //     {
    //         system.Stop();
    //     }
    //
    //     foreach (VisualEffect visualEffect in visualEffects)
    //     {
    //         visualEffect.Stop();
    //     }
    // }

    public void OnInteract(NpcController npc = null)
    {
        onInteractEvent.Invoke();
    }

    public void OnDeposit()
    {
        onDepositEvent.Invoke();
    }

    public void OnFinishTask()
    {
        onFinishEvent.Invoke();
    }

    public void OnInterruptTask()
    {
        onInterruptEvent.Invoke();
    }
}
