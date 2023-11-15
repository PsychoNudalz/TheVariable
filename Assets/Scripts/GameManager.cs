using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomLabel
{
    None,
    LivingRoom,
    Garage,
    StaffToilet,
    Storage,
    ServantRoom,
    GuardRoom,
    Study,
    MasterBedroom,
    Corridor,
    Connector,
    Kitchen
    
}

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        None,
        Tutorial,
        Free,
        GameOver,
        GameWin
    }
    
    
    public static GameManager GM;
    private GameState gameState;
    
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
    private float pausedTime;

    enum TimerState
    {
        None,
        Started,
        Pause,
        Finished
    }

    private TimerState timerState;

    private static float currentTimeScale => Time.timeScale;
    private static float hackSlowScale = .1f;
    public static bool RanOutOfTime => GM.timerState == TimerState.Finished;

    [Header("Objective")]
    [SerializeField]
    private bool isVIPDead = false;

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
    void Update()
    {
        if (timerState == TimerState.Started)
        {
            globalTimeRemaining = globalRealTimer - (Time.realtimeSinceStartup - GM.globalStartTime);
            UIController.current.UpdateTimer(globalTimeRemaining);
            if (globalTimeRemaining < 0)
            {
                timerState = TimerState.Finished;
            }
        }
    }

    public static void Alert_Suspicious()
    {
        SoundManager.PlayGlobal(SoundGlobal.Suspicious);
    }

    public static void SlowTimeForHack()
    {
        Time.timeScale = hackSlowScale;
    }public static void StopTime()
    {
        Time.timeScale = 0;
    }

    [Command()]
    public static void ResetTimeScale()
    {
        Time.timeScale = 1;
    }


    public static void StartTimer()
    {
        GM.timerState = TimerState.Started;
        GM.globalStartTime = Time.realtimeSinceStartup;
        UIController.current.StartTimer(GM.globalRealTimer);
    }

    public static void PauseTimer(bool b)
    {
        if (b)
        {
            GM.timerState = TimerState.Pause;
            GM.pausedTime = Time.realtimeSinceStartup;
        }
        else
        {
            GM.timerState = TimerState.Started;
            GM.globalStartTime += Time.realtimeSinceStartup - GM.pausedTime;
        }
    }

    public static void GameOver()
    {
        if (GM.gameState is GameState.GameOver or GameState.GameWin)
        {
            return;
        }
        //Get Score from player pref
        float highScore = PlayerPrefs.GetFloat("Time");
        if (highScore == 0f)
        {
            highScore = Mathf.Infinity;
        }
        float runTime = Time.realtimeSinceStartup - GM.globalStartTime;
        if (runTime < highScore)
        {
            highScore = runTime;
            PlayerPrefs.SetFloat("Time",highScore);
        }

        UIController.current.GameOver(runTime, highScore);
        Debug.Log("GAME OVER");
        GM.gameState = GameState.GameOver;
    }

    public static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Command()]
    public static void ResetSave()
    {
        PlayerPrefs.SetFloat("Time",Mathf.Infinity);

    }

    [Command()]
    public static void DebugMode()
    {
        MaxLevel();
        ResetTimeScale();
    }

    [Command()]
    public static void MaxLevel()
    {
        PlayerController.current.IncreaseClearanceLevel(3);
    }

    public static void KillVIP()
    {
        GM.isVIPDead = true;
        Debug.Log("VIP IS KILLED");
        UIController.current.Objective_KilledVIP();
    }
}