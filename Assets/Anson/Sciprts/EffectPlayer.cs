using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

/// <summary>
/// play effects mostly for calling unity events inside an animation
/// </summary>
public class EffectPlayer : MonoBehaviour
{
    [SerializeField]
    private SoundAbstract[] sounds;

    [SerializeField]
    ParticleSystem[] particleSystems;

    [SerializeField]
    VisualEffect[] visualEffects;

    [SerializeField]
    UnityEvent[] unityEvents;

    [SerializeField]
    [TextArea(5, 20)]
    private string Notes;

    public void PlayPS(int index)
    {
        if (index < particleSystems.Length)
        {
            particleSystems[index].Stop();
            particleSystems[index].Play();
        }
    }

    public void StopPS(int index)
    {
        if (index < particleSystems.Length)
        {
            particleSystems[index].Stop();
        }
    }

    public void PlayVE(int index)
    {
        if (index < visualEffects.Length)
        {
            visualEffects[index].Stop();
            visualEffects[index].Play();
        }
    }

    public void StopVE(int index)
    {
        if (index < visualEffects.Length)
        {
            visualEffects[index].Stop();
        }
    }

    public void PlayUE(int index)
    {
        if (index < unityEvents.Length)
        {
            unityEvents[index].Invoke();
        }
    }

    public void Sound_Play(int index)
    {
        if (index < sounds.Length)
        {
            sounds[index].PlayF();
        }
    }

    public void Sound_Stop(int index)
    {
        if (index < sounds.Length)
        {
            sounds[index].Stop();
        }
    }

    public void Reset()
    {
        sounds = Array.Empty<SoundAbstract>();
        unityEvents = Array.Empty<UnityEvent>();
        particleSystems = Array.Empty<ParticleSystem>();
        visualEffects = Array.Empty<VisualEffect>();
    }
}