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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
