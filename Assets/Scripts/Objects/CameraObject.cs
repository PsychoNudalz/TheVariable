using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraObject : SmartObject
{
    [Header("Camera")]
    [SerializeField]
    private CinemachineVirtualCamera camera;
    [SerializeField]
    private Transform camera_transform;

    [SerializeField]
    private Vector2 xClamp = new Vector2(-90f, 90f);
    private Vector3 cameraOrientation = default;

    private const int CameraPriority = 10;
    public override Vector3 Forward =>camera_transform.forward;

    protected override void AwakeBehaviour()
    {
        cameraOrientation = camera_transform.eulerAngles;
        SetActive(false);
    }

    protected override void StartBehaviour()
    {
    }

    protected override void UpdateBehaviour()
    {
    }

    public override void Interact(NpcController npc)
    {
    }



    public void RotateCamera(float horizontal, float vertical)
    {
        cameraOrientation.x = Mathf.Clamp(cameraOrientation.x - vertical, xClamp.x, xClamp.y);
        cameraOrientation.y += horizontal;
        camera_transform.eulerAngles = cameraOrientation;

    }

    public void SetActive(bool b)
    {
        if (b)
        {
            camera.Priority = CameraPriority;
        }
        else
        {
            camera.Priority = -1;
        }
    }
    
    
}
