using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A system that shares data to all NPCs
/// </summary>
public class GlobalKnowledgeSystem
{
    [Header("Current")]
    static CameraObject player_Camera;
    
    [Header("Last Known")]
    static Vector3 player_LastKnown_Position = new Vector3();
    static CameraObject player_LastKnown_Camera;
    static float player_LastKnown_Time = 0;

    public static Vector3 PlayerLastKnownPosition => player_LastKnown_Position;

    public static CameraObject PlayerLastKnownCamera => player_LastKnown_Camera;

    public static float PlayerLastKnownTime => player_LastKnown_Time;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void SpottedPlayer(Vector3 position, CameraObject cameraObject, float time)
    {
        player_LastKnown_Position = position;
        player_LastKnown_Camera = cameraObject;
        player_LastKnown_Time = time;
    }

    public static void UpdatePlayerCamera(CameraObject newCamera)
    {
        player_Camera = newCamera;
    }
}