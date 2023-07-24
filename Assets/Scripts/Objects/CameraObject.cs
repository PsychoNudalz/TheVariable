using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraObject : SmartObject
{
    [Header("Camera")]
    [Header("Movement")]
    [SerializeField]
    private CinemachineVirtualCamera camera;

    [SerializeField]
    private Transform camera_transform;

    [SerializeField]
    private Vector2 xClamp = new Vector2(-90f, 90f);

    [Header("Zone")]
    [SerializeField]
    [Range(0f, 1f)]
    private float zoomLevel = 0f;
    

    [SerializeField]
    private float zoomFOVMultiplier = .5f;

    private float originalFOV;

    [SerializeField]
    private float zoomSpeed = 1f;

    [Space(10)]
    [Header("Components")]
    [SerializeField]
    private Renderer cameraBody;

    [SerializeField]
    private Collider collider;

    private Vector3 cameraOrientation = default;

    private const int CameraPriority = 10;
    public override Vector3 Position => camera_transform.position;
    public override Vector3 Forward => camera_transform.forward;


    protected override void AwakeBehaviour()
    {
        cameraOrientation = camera_transform.eulerAngles;
        originalFOV = camera.m_Lens.FieldOfView;
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
        float zoomRotateMultiplier = Mathf.Lerp(1,.2f, zoomLevel);
        cameraOrientation.x = Mathf.Clamp(cameraOrientation.x - vertical*zoomRotateMultiplier, xClamp.x, xClamp.y);
        cameraOrientation.y += horizontal*zoomRotateMultiplier;
        camera_transform.eulerAngles = cameraOrientation;
    }

    public void UpdateZoom(float zoomAmount)
    {
        zoomLevel = Mathf.Clamp(zoomAmount * Time.deltaTime + zoomLevel, 0f, 1f);
        camera.m_Lens.FieldOfView = Mathf.Lerp(originalFOV,originalFOV*zoomFOVMultiplier,zoomLevel);

    }

    public void SetActive(bool b)
    {
        if (b)
        {
            camera.Priority = CameraPriority;
            if (collider)
            {
                collider.enabled = false;
            }

            if (cameraBody)
            {
                cameraBody.enabled = false;
            }
        }
        else
        {
            camera.Priority = -1;
            if (collider)
            {
                collider.enabled = true;
            }

            if (cameraBody)
            {
                cameraBody.enabled = true;
            }
        }
    }
}