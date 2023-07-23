using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    enum ControlMode
    {
        MK,
        Controller
    }

    enum ZoomMode
    {
        None,
        In,
        Out
    }

    private ControlMode controlMode = ControlMode.MK;
    private ZoomMode zoomMode = ZoomMode.None;

    [Header("Components")]
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private CameraObject currentCamera;

    [Header("Settings")]
    [SerializeField]
    private float rotateMultiplier = 1f;

    [SerializeField]
    private float rotateMultiplier_joystick = 3f;

    [Header("Selector")]
    [SerializeField]
    [Space(5)]
    private SmartObject selectedObject;

    [SerializeField]
    private LayerMask selectorLayer;

    [SerializeField]
    private float castRange = 20f;


    private Vector2 lookValue;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentCamera = cameraManager.Cameras[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode == ControlMode.Controller)
        {
            if (lookValue.magnitude > 0.1f)
            {
                UpdateCamera(lookValue);
            }
        }

        switch (zoomMode)
        {
            case ZoomMode.None:
                currentCamera.UpdateZoom(0);

                break;
            case ZoomMode.In:
                currentCamera.UpdateZoom(1);

                break;
            case ZoomMode.Out:
                currentCamera.UpdateZoom(-1);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FixedUpdate()
    {
        RaycastCamera();

    }

    public void OnLook(InputValue inputValue)
    {
        controlMode = ControlMode.MK;
        lookValue = inputValue.Get<Vector2>() * rotateMultiplier;
        UpdateCamera(lookValue);
    }

    public void OnLook_Joystick(InputValue inputValue)
    {
        controlMode = ControlMode.Controller;
        lookValue = inputValue.Get<Vector2>() * rotateMultiplier_joystick;
    }


    public void OnCamera_Next()
    {
        currentCamera = cameraManager.GetNextCamera(currentCamera);
    }

    public void OnCamera_Prev()
    {
        currentCamera = cameraManager.GetPrevCamera(currentCamera);
    }


    public void OnZoom(InputValue inputValue)
    {
        float zoom = inputValue.Get<float>();

        if (zoom > 0.1f)
        {
            zoomMode = ZoomMode.In;
        }
        else if (zoom < -0.1f)
        {
            zoomMode = ZoomMode.Out;
        }
        else
        {
            zoomMode = ZoomMode.None;
        }
    }
    void UpdateCamera(Vector2 rotation)
    {
        if (currentCamera)
        {
            currentCamera.RotateCamera(rotation.x, rotation.y);
        }
    }

    void RaycastCamera()
    {
        RaycastHit hit;
        if (Physics.Raycast(currentCamera.Position, currentCamera.Forward,out hit, castRange, selectorLayer))
        {
            SmartObject smartObject = hit.collider.GetComponentInParent<SmartObject>();
            if (smartObject)
            {
                if (!smartObject.Equals(selectedObject))
                {
                    if (selectedObject)
                    {
                        selectedObject.OnSelect_Exit();
                    }
                    selectedObject = smartObject;
                    selectedObject.OnSelect_Enter();
                }
            }
        }
        else
        {
            if (selectedObject)
            {
                selectedObject.OnSelect_Exit();
                selectedObject = null;
            }
        }
    }
}