using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using HighlightPlus;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;

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

    [Header("Effects")]
    [SerializeField]
    private HighlightEffect throughWallEffect;

    [SerializeField]
    private float throughWallEffect_Time = 1f;

    private float throughWallEffect_TimeNow = 0;
    private bool throughWallEffect_Active = false;

    [Space(10)]
    [Header("Components")]
    [SerializeField]
    private Renderer cameraBody;

    [FormerlySerializedAs("collider")]
    [SerializeField]
    private Collider disableCollider;
    [SerializeField]
    Collider mainCollider;


    private Vector3 cameraOrientation = default;

    private const int CameraPriority = 10;

    private bool playerControl = false;
    public override Vector3 Position => camera_transform.position;
    public override Vector3 Forward => camera_transform.forward;

    public override Vector3 ColliderPosition => mainCollider.transform.position;

    public bool PlayerControl => playerControl;

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
        if (throughWallEffect_Active)
        {
            ThroughWallEffect_Update();
        }
    }

    public override void Interact(NpcController npc)
    {
    }

    private void OnApplicationQuit()
    {
        camera.m_Lens.FieldOfView = originalFOV;
    }

    private void OnDestroy()
    {
        camera.m_Lens.FieldOfView = originalFOV;
    }

    public void RotateCamera(float horizontal, float vertical)
    {
        float zoomRotateMultiplier = Mathf.Lerp(1, .2f, zoomLevel);
        cameraOrientation.x = Mathf.Clamp(cameraOrientation.x - vertical * zoomRotateMultiplier, xClamp.x, xClamp.y);
        cameraOrientation.y += horizontal * zoomRotateMultiplier;
        camera_transform.eulerAngles = cameraOrientation;
    }

    public void UpdateZoom(float zoomAmount)
    {
        zoomLevel = Mathf.Clamp(zoomAmount * Time.deltaTime + zoomLevel, 0f, 1f);
        camera.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, originalFOV * zoomFOVMultiplier, zoomLevel);
    }

    public void SetActive(bool b)
    {
        if (b)
        {
            camera.Priority = CameraPriority;
            if (disableCollider)
            {
                disableCollider.enabled = false;
            }

            if (cameraBody)
            {
                cameraBody.enabled = false;
            }
        }
        else
        {
            camera.Priority = -1;
            if (disableCollider)
            {
                disableCollider.enabled = true;
            }

            if (cameraBody)
            {
                cameraBody.enabled = true;
            }
        }

        playerControl = b;
    }

    public void ThroughWallEffect_Activate()
    {
        throughWallEffect_Active = true;
        throughWallEffect_TimeNow = 0;
        throughWallEffect.highlighted = true;
    }

    void ThroughWallEffect_Update()
    {
        if (!throughWallEffect_Active)
        {
            return;
        }

        if (throughWallEffect_TimeNow > 1)
        {
            throughWallEffect_Active = false;
            throughWallEffect_TimeNow = 0;
            throughWallEffect.highlighted = false;

            return;
        }

        throughWallEffect_TimeNow += 1f / throughWallEffect_Time * Time.deltaTime;

        throughWallEffect.overlay = Mathf.Sin(throughWallEffect_TimeNow * 2 * Mathf.PI);
    }
}