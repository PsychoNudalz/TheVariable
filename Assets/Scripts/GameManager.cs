using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    private float suspicious_LastTime = 0;
    private float suspicious_Cooldown = 5;
    [Header("Global Sounds")]
    [SerializeField]
    private SoundAbstract sfx_Suspicious;

    [Space(10)]
    [Header("Timer")]
    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float globalRealTimer = 300f;

    // [SerializeField]
    private float globalStartTime = 0f;
    private float globalTimeRemaining;

    private bool isTimerOn = false;
    
    private static float currentTimeScale => Time.timeScale;
    private static float hackSlowScale = .1f;
    private void Awake()
    {
        if (!GM)
        {
            GM = this;
        }
        else
        {
            Destroy(GM);
            GM = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO:Remove this and move to tutorial
        StartTimer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTimerOn)
        {
            globalTimeRemaining = globalRealTimer - (Time.realtimeSinceStartup - GM.globalStartTime);
            UIController.current.UpdateTimer(globalTimeRemaining);
        }
    }

    public static void Alert_Suspicious()
    {
        if (Time.time - GM.suspicious_LastTime > GM.suspicious_Cooldown)
        {
            if (!GM.sfx_Suspicious.IsPlaying())
            {
                GM.sfx_Suspicious.Play();
            }
        }

        GM.suspicious_LastTime = Time.time;

    }

    public static void SlowTimeForHack()
    {
        Time.timeScale = hackSlowScale;
    }

    public static void ResetTimeScale()
    {
        Time.timeScale = 1;
    }

    public static void StartTimer()
    {
        GM.isTimerOn = true;
        GM.globalStartTime = Time.realtimeSinceStartup;
        UIController.current.StartTimer(GM.globalRealTimer);
    }
}
