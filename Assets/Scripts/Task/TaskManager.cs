using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Keeps tracks of world times
/// Handles tasks
/// </summary>
public class TaskManager : MonoBehaviour
{
    [SerializeField]
    private int currentTick;

    [Space(15)]
    [SerializeField]
    private int TickLoop = 48;

    [SerializeField]
    private float TimePerTick = 30;

    private float timePerTickCurrent;

    public static TaskManager current;
    public static int Tick => current.currentTick % current.TickLoop;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timePerTickCurrent += Time.deltaTime;
        if (timePerTickCurrent > TimePerTick)
        {
            currentTick++;
            timePerTickCurrent -= TimePerTick;
        }
    }
}