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

    enum CameraMode
    {
        Free,
        SelectHack
    }

    private ControlMode controlMode = ControlMode.MK;
    private ZoomMode zoomMode = ZoomMode.None;
    private CameraMode cameraMode = CameraMode.Free;

    [Header("Components")]
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private CameraObject currentCamera;

    [SerializeField]
    private UIController uiController;

    [Header("Settings")]
    [SerializeField]
    private float rotateMultiplier = 1f;

    [SerializeField]
    private float rotateMultiplier_joystick = 3f;

    [SerializeField]
    private float zoomMultiplier = 10f;

    [SerializeField]
    private float zoomMultiplier_joystick = 1f;

    [Header("Selector")]
    [SerializeField]
    [Space(5)]
    private SmartObject selectedObject;

    [SerializeField]
    private LayerMask selectorLayer;

    [SerializeField]
    private float castRange = 20f;


    private Vector2 lookValue;

    [SerializeField]
    private Vector2 selectDir;

    public static PlayerController current;

    private void Awake()
    {
        current = this;
    }

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
            if (lookValue.magnitude > 0.01f*rotateMultiplier_joystick)
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
                switch (controlMode)
                {
                    case ControlMode.MK:
                        currentCamera.UpdateZoom(zoomMultiplier);

                        break;
                    case ControlMode.Controller:
                        currentCamera.UpdateZoom(zoomMultiplier_joystick);

                        break;
                }

                break;
            case ZoomMode.Out:
                switch (controlMode)
                {
                    case ControlMode.MK:
                        currentCamera.UpdateZoom(-zoomMultiplier);

                        break;
                    case ControlMode.Controller:
                        currentCamera.UpdateZoom(-zoomMultiplier_joystick);

                        break;
                }

                break;
        }
    }

    private void FixedUpdate()
    {
        RaycastCamera();

        if (cameraMode == CameraMode.SelectHack)
        {
            uiController.HacksDisplay_UpdateDir(selectDir);
        }
    }



    //Player Input
    public void OnLook(InputValue inputValue)
    {
        if (cameraMode == CameraMode.SelectHack)
        {
            return;
        }

        controlMode = ControlMode.MK;
        lookValue = inputValue.Get<Vector2>() * rotateMultiplier;
        UpdateCamera(lookValue);
    }

    public void OnLook_Joystick(InputValue inputValue)
    {
        if (cameraMode == CameraMode.SelectHack)
        {
            return;
        }

        controlMode = ControlMode.Controller;
        lookValue = inputValue.Get<Vector2>() * rotateMultiplier_joystick;
    }


    public void OnSelectHack(InputValue inputValue)
    {
        if (cameraMode == CameraMode.Free)
        {
            return;
        }

        controlMode = ControlMode.MK;
        Vector2 dir = inputValue.Get<Vector2>();
        if (dir.magnitude > .2f)
        {
            selectDir = dir;
        }
    }

    public void OnSelectHack_Joystick(InputValue inputValue)
    {
        if (cameraMode == CameraMode.Free)
        {
            return;
        }

        controlMode = ControlMode.Controller;
        selectDir = inputValue.Get<Vector2>();
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

    public void OnSelect(InputValue inputValue)
    {
        if (!selectedObject)
        {
            cameraMode = CameraMode.Free;
            return;
        }

        if (inputValue.isPressed)
        {
            switch (cameraMode)
            {
                case CameraMode.Free:
                    DisplayHack();
                    break;
                case CameraMode.SelectHack:

                    break;
            }
        }
        else
        {
            switch (cameraMode)
            {
                case CameraMode.Free:
                    break;
                case CameraMode.SelectHack:
                    SelectHack(selectDir);

                    break;
            }
        }
    }

    private void SelectHack(Vector2 dir)
    {
        uiController.HacksDisplay_SelectHack(selectDir);
        cameraMode = CameraMode.Free;
        uiController.HacksDisplay_SetActive(false);
    }

    private void DisplayHack()
    {
        cameraMode = CameraMode.SelectHack;
        uiController.HacksDisplay_SetActive(true, selectedObject);
    }
    //Player Input END


    public void ChangeCamera(CameraObject cameraObject)
    {
        currentCamera = cameraManager.ChangeCamera(cameraObject, currentCamera);
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
        if (Physics.Raycast(currentCamera.Position, currentCamera.Forward, out hit, castRange, selectorLayer))
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

                Debug.DrawRay(currentCamera.Position, currentCamera.Forward * castRange, Color.green, Time.deltaTime);
            }
        }
        else
        {
            //TODO - might need to check if it is worth disabling the hack wheel when it can't detect it no more
            if (selectedObject)
            {
                selectedObject.OnSelect_Exit();
                selectedObject = null;
            }

            Debug.DrawRay(currentCamera.Position, currentCamera.Forward * castRange, Color.green, Time.deltaTime);
        }
    }
}