using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Task;
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
    public enum GameState
    {
        None,
        Tutorial,
        Free,
        PauseGame,
        GameOver,
        GameWin
    }


    public static GameManager GM;
    private GameState gameState;

    private float suspicious_LastTime = 0;
    private float suspicious_Cooldown = 5;
    [Header("Clearance Colours")]
    [SerializeField]
    private Color[] clearanceColours = new Color[4];
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

    [SerializeField]
    private int maxGB = 0;

    public int MaxGb => maxGB;

    public static bool IsVIPDead => GM.isVIPDead;


    public bool IsPaused => gameState is GameState.Tutorial or GameState.PauseGame;
    // [Header("NPCs")]
    // [SerializeField]
    // private NpcManager npcManager;
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

        ResetTimeScale();
    }

    // Start is called before the first frame update
    void Start()
    {
        //TODO:Remove this and move to tutorial
        StartTimer();
        UIController.current.Objective_SetDataMax(maxGB);
        GM.gameState = GameState.Free;
        CursorLock(true);
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

    [ContextMenu("CalculateMaxGB")]
    public void CalculateMaxGB()
    {
        maxGB = 0;
        SmartObject[] allSOs = FindObjectsByType<SmartObject>(FindObjectsInactive.Exclude,FindObjectsSortMode.InstanceID);
        List<SmartObject> evaluated = new List<SmartObject>();
        foreach (SmartObject smartObject in allSOs)
        {
            if (evaluated.Contains(smartObject))
            {
                Debug.LogError($"2 copies of SO: {smartObject}");
                break;
            }
            evaluated.Add(smartObject);

            foreach (HackAbility hackAbility in smartObject.Hacks)
            {
                if (hackAbility is Hack_Collect_Data data)
                {
                    if (smartObject is TaskSmartObject ts)
                    {
                        Debug.Log($"Gold Task Object: {ts}");

                    }
                    maxGB += data.Gb;
                }
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
    }

    public static void StopTime()
    {
        Time.timeScale = 0;
    }

    [Command()]
    public static void ResetTimeScale()
    {
        Time.timeScale = 1;
        GM.gameState = GameState.Free;

    }


    public static void StartTimer()
    {
        GM.timerState = TimerState.Started;
        GM.globalStartTime = Time.realtimeSinceStartup;
        UIController.current.StartTimer(GM.globalRealTimer);
    }

    public static void PauseTimer(bool b)
    {
        if (b&&GM.timerState == TimerState.Started)
        {
            GM.timerState = TimerState.Pause;
            GM.pausedTime = Time.realtimeSinceStartup;
        }
        else if(!b&&GM.timerState == TimerState.Pause)
        {
            GM.timerState = TimerState.Started;
            GM.globalStartTime += Time.realtimeSinceStartup - GM.pausedTime;
        }
    }

    [Command()]
    public static void GameOver()
    {
        NpcManager.SetActive(false);
        PlayerController.current.LockInput(true);
        if (GM.gameState is GameState.GameOver or GameState.GameWin)
        {
            return;
        }

        //Get Score from player pref
        //Time
        float fastestTime = PlayerPrefs.GetFloat("Time");
        // if (fastestTime == 0f)
        // {
        //     fastestTime = Mathf.Infinity;
        // }
        //
        
        //Score
        float highScore = PlayerPrefs.GetFloat("Score");
        float runTime = Time.realtimeSinceStartup - GM.globalStartTime;


        float currentCollectedGb = PlayerController.current.CollectedGb;
        UIController.current.GameOver(runTime, fastestTime,currentCollectedGb,highScore);
        Debug.Log("GAME OVER");
        GM.gameState = GameState.GameOver;
    }

    [Command()]
    public static void GameWin()
    {
        NpcManager.SetActive(false);
        PlayerController.current.LockInput(true);

        if (GM.gameState is GameState.GameOver or GameState.GameWin)
        {
            return;
        }

        //Time
        float fastestTime = PlayerPrefs.GetFloat("Time");
        // if (fastestTime == 0f)
        // {
        //     fastestTime = 9999999999f;
        // }
        float runTime = Time.realtimeSinceStartup - GM.globalStartTime;

        //Score
        float highScore = PlayerPrefs.GetFloat("Score");
        int playerGB = PlayerController.current.CollectedGb;

        if (playerGB > highScore)
        {
            fastestTime = runTime;
            PlayerPrefs.SetFloat("Time", fastestTime);
            highScore = playerGB;
            PlayerPrefs.SetFloat("Score", highScore);

        }
        
        UIController.current.GameWin(runTime, fastestTime,playerGB,highScore);
        Debug.Log("GAME WIN");
        GM.gameState = GameState.GameWin;
    }
    [Command()]

    public static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Command()]
    public static void ResetSave()
    {
        PlayerPrefs.SetFloat("Time", 0);
        PlayerPrefs.SetFloat("Score", 0);
    }

    [Command()]
    public static void DebugMode()
    {
        MaxLevel();
        ResetTimeScale();
        TutorialManager.SetTutorial(false);
    }

    [Command()]
    public static void MaxLevel()
    {
        PlayerController.current.IncreaseClearanceLevel(3);
    }

    [Command()]
    public static void Kill_VIP()
    {
        NpcManager.GetVIP()?.NpcObject.Hack_Delete();
    }

    public static void VIPDead()
    {
        GM.isVIPDead = true;
        Debug.Log("VIP IS KILLED");
        UIController.current.Objective_KilledVIP();
        UIController.current.Objective_ShowExtraction();
    }



    [Command()]
    public static void Sensitivity_Joystick(float m)
    {
        PlayerController.current.SetJoystickMultiplier(m);
    }

    [Command()]
    public static void AddGB(int i)
    {
        PlayerController.current.AddGB(i);
    }

    public static Color GetClearanceColour(int i)
    {
        return GM.clearanceColours[Mathf.Min(i, GM.clearanceColours.Length)];
    }

    [Command()]
    public static void OverrideHackSpeedUp(float speed)
    {
        PlayerController.current.OverrideSpeedUp(speed);
    }

    [Command()]
    public static void TogglePause()
    {
        if (GM.gameState == GameState.PauseGame)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    [Command()]
    public static void PauseGame()
    {
        TutorialManager.current.OnWindow_Close();
        
        PlayerController.current.LockInput(true);
        
        StopTime();
        CursorLock(false);
        PauseTimer(true);

        UIController.current.PauseGame_SetActive(true);
        
        ChangeState(GameState.PauseGame);
    }
    
    [Command()]
    public static void ResumeGame()
    {
        PlayerController.current.LockInput(false);
        
        ResetTimeScale();
        CursorLock(true);
        PauseTimer(false);

        UIController.current.PauseGame_SetActive(false);
        
        ChangeState(GameState.Free);

    }

    [Command()]
    public static void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    

    /// <summary>
    /// Locks and disable cursor visibility
    /// 
    /// </summary>
    /// <param name="b"></param>
    [Command()]
    public static void CursorLock(bool b)
    {
        if (b)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void ChangeState(GameState gameState)
    {
        GM.gameState = gameState;
    }
    
    
    //----------------------TEST CASES---------------
    /// <summary>
    /// Test Case 1: add GB and Game Win
    /// </summary>
    [Command()]
    public static void TC_1()
    {
        AddGB(12345);
        GameWin();
        // GM.Invoke(nameof(GameWin),.1f);
    }
    
    /// <summary>
    /// Test Case 2: add GB and Game Over
    /// </summary>
    [Command()]
    public static void TC_2()
    {
        AddGB(12345);
        GameOver();
        // GM.Invoke(nameof(GameOver),.1f);

    }
    
    
}