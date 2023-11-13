using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public enum SoundGlobal
{
    Suspicious,
    Spotted,
    Hacking,
    HackComplete,
    Lockout,
    SwitchCamera,
    Tutorial_On,
    Distraction
    
}

[Serializable]
public class GlobalSoundPair
{
    [SerializeField]
    private SoundGlobal soundGlobal;

    [SerializeField]
    private SoundAbstract sound;

    [SerializeField]
    private float delayPlay = 0;

    [SerializeField]
    private float cooldownTime = 1;

    [SerializeField]
    private float lastPlayedTime = -100;

    public SoundGlobal SoundGlobal => soundGlobal;

    public SoundAbstract Sound => sound;


    public GlobalSoundPair(SoundGlobal soundGlobal, SoundAbstract sound)
    {
        this.soundGlobal = soundGlobal;
        this.sound = sound;
    }

    public void Play()
    {
        if (Time.time - lastPlayedTime > cooldownTime)
        {
            lastPlayedTime = Time.time;
            if (delayPlay > 0)
            {
                sound.Play(delayPlay);
            }
            else
            {
                sound.PlayF();
            }
        }
    }

    public void Stop()
    {
        sound.Stop();
    }
}

/// <summary>
/// 13th generation of the sound manager
///
/// Will be modified for the game
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager current;

    [SerializeField]
    List<SoundAbstract> sounds = new List<SoundAbstract>();

    [SerializeField]
    List<SoundAbstract> soundsCache = new List<SoundAbstract>();

    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    bool playBGM = true;

    [SerializeField]
    SoundAbstract bgm;

    [SerializeField]
    private GlobalSoundPair[] globalSoundPairs;

    private Dictionary<SoundGlobal, GlobalSoundPair> globalSoundDict = new Dictionary<SoundGlobal, GlobalSoundPair>();

    // private Dictionary<SoundGlobal, >
    private void Awake()
    {
        if (current)
        {
            Destroy(current);
        }

        current = this;
        foreach (GlobalSoundPair globalSoundPair in globalSoundPairs)
        {
            globalSoundDict.Add(globalSoundPair.SoundGlobal, globalSoundPair);
        }
    }

    private void Start()
    {
        UpdateSounds();
        if (bgm != null && playBGM)
        {
            if (!bgm.IsPlaying())
            {
                bgm.Play();
            }
        }
    }


    [ContextMenu("Update Sounds")]
    public void UpdateSounds()
    {
        List<SoundAbstract> newSounds = new List<SoundAbstract>(FindObjectsOfType<SoundAbstract>());
        sounds = new List<SoundAbstract>();
        foreach (SoundAbstract s in newSounds)
        {
            AddSounds(s);
            s.SoundManager = this;
        }
    }

    public void AddSounds(SoundAbstract sa)
    {
        if (!sounds.Contains(sa))
        {
            if (sa is Sound item)
            {
                if (item.AudioMixer == null)
                {
                    item.AudioMixer = audioMixer;
                }

                sounds.Add(item);
            }
        }
    }

    public void PauseAllSounds()
    {
        UpdateSounds();
        soundsCache = new List<SoundAbstract>();
        foreach (SoundAbstract s in sounds)
        {
            if (s.IsPlaying())
            {
                soundsCache.Add(s);
                s.Pause();
            }
        }
    }

    public void ResumeSounds()
    {
        foreach (SoundAbstract s in soundsCache)
        {
            s.Resume();
        }

        UpdateSounds();
    }


    public void StopAllSounds()
    {
        soundsCache = new List<SoundAbstract>();
        foreach (SoundAbstract s in sounds)
        {
            if (s != null && s.IsPlaying())
            {
                soundsCache.Add(s);
                s.Stop();
            }
        }
    }

    private void OnDestroy()
    {
        StopAllSounds();
    }

    public void Play(SoundGlobal soundGlobal)
    {
        try
        {
            globalSoundDict[soundGlobal].Play();
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError($"Sound manager can't play: {soundGlobal}");
        }
    }

    public void Stop(SoundGlobal soundGlobal)
    {
        try
        {
            globalSoundDict[soundGlobal].Stop();
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError($"Sound manager can't play: {soundGlobal}");
        }
    }

    public static void PlayGlobal(SoundGlobal soundGlobal)
    {
        current.Play(soundGlobal);
    }

    public static void StopGlobal(SoundGlobal soundGlobal)
    {
        current.Stop(soundGlobal);
    }
}