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

    [Header("Selector")]
    [SerializeField]
    [Space(5)]
    private SmartObject selectedObject;

    [SerializeField]
    private LayerMask selectorLayer;

    [SerializeField]
    private float castRange = 20f;


    private Vector2 lookValue;

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
                    cameraMode = CameraMode.SelectHack;
                    uiController.Display_Hacks(true,selectedObject);
                    break;
                case CameraMode.SelectHack:
                    cameraMode = CameraMode.Free;
                    uiController.Display_Hacks(false);
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
                    cameraMode = CameraMode.Free;
                    uiController.Display_Hacks(false);

                    break;
            }
        }
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
                Debug.DrawRay(currentCamera.Position, currentCamera.Forward*castRange,Color.green,Time.deltaTime);
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
            Debug.DrawRay(currentCamera.Position, currentCamera.Forward*castRange,Color.green,Time.deltaTime);

        }
    }
    
}