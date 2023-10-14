using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
        SelectHack,
        LockedOut
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
    private List<CameraObject> cameraStack = new List<CameraObject>(10);

    private int cameraStackIndex = 0;
    CameraObject currentCameraFromStack => cameraStack[cameraStackIndex];

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
    private LayerMask cameraLayer;

    [SerializeField]
    private float camera_CastRange = 20f;


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
        ChangeCamera(cameraManager.Cameras[0]);
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating camera
        if (cameraMode != CameraMode.LockedOut)
        {
            if (cameraMode == CameraMode.Free && controlMode == ControlMode.Controller)
            {
                if (lookValue.magnitude > 0.01f * rotateMultiplier_joystick)
                {
                    UpdateCamera(lookValue);
                }
            }
        }

        //Zooming
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
        switch (cameraMode)
        {
            case CameraMode.Free:
                RaycastCamera();
                break;
            case CameraMode.SelectHack:
                uiController.HacksDisplay_UpdateDir(selectDir);
                break;
            case CameraMode.LockedOut:
                if (!currentCamera.IsLocked)
                {
                    DeactivateLockout();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    //Player Input
    public void OnLook(InputValue inputValue)
    {
        if (cameraMode != CameraMode.Free)
        {
            return;
        }

        controlMode = ControlMode.MK;
        lookValue = inputValue.Get<Vector2>() * rotateMultiplier;
        UpdateCamera(lookValue);
    }

    public void OnLook_Joystick(InputValue inputValue)
    {
        if (cameraMode != CameraMode.Free)
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


    public void OnCameraManager_Cycle()
    {
        currentCamera = cameraManager.GetNextCamera(currentCamera);
    }

    public void OnCamera_Next()
    {
        if (cameraStack.Count == 0)
        {
            return;
        }

        if (cameraMode == CameraMode.SelectHack)
        {
            return;
        }

        if (cameraStackIndex == 0)
        {
            return;
        }

        cameraStackIndex--;
        cameraStackIndex = (int) Math.Clamp(cameraStackIndex, 0, cameraStack.Count - 1);
        // ChangeCamera(cameraStack[cameraStackIndex],false);
        // currentCamera = cameraManager.GetNextCamera(currentCamera);
        SwitchToStackCamera();

    }

    public void OnCamera_Prev()
    {
        if (cameraStack.Count == 0)
        {
            return;
        }

        if (cameraMode == CameraMode.SelectHack)
        {
            return;
        }

        if (cameraStackIndex == cameraStack.Count - 1)
        {
            return;
        }

        cameraStackIndex++;
        cameraStackIndex = (int) Math.Clamp(cameraStackIndex, 0, cameraStack.Count - 1);

        SwitchToStackCamera();

        // ChangeCamera(cameraStack[cameraStackIndex],false);
        // cameraStack[cameraStackIndex].ActivateHack<Hack_Camera_Switch>(new HackContext_Enum[1]
        // {
        //     HackContext_Enum.Camera_notPushToStack
        // });

        // currentCamera = cameraManager.GetPrevCamera(currentCamera);
    }

    private void SwitchToStackCamera()
    {
        currentCamera.StartHack(currentCameraFromStack, currentCamera.GetHackIndex<Hack_Camera_Switch>(),
            new HackContext_Enum[1]
            {
                HackContext_Enum.Camera_notPushToStack
            });
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

    public void OnSelectCancel()
    {
        switch (cameraMode)
        {
            case CameraMode.Free:
                break;
            case CameraMode.SelectHack:
                SelectHack(Vector2.zero);
                break;
        }
    }

    public void OnHighlightCameras()
    {
        cameraManager.ActiveThroughWalls(currentCamera.Position, camera_CastRange);
    }

    public void OnHoldHack(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            print("Override to set hacking");
            currentCamera.Override_IsHacking();
        }
    }

    private void SelectHack(Vector2 dir)
    {
        int hackIndex = uiController.HacksDisplay_SelectHack(dir);
        if (hackIndex >= 0)
        {
            currentCamera.StartHack(selectedObject, hackIndex);
        }

        cameraMode = CameraMode.Free;
        uiController.HacksDisplay_SetActive(false);
    }


    private void DisplayHack()
    {
        cameraMode = CameraMode.SelectHack;
        selectDir = Vector2.zero;
        uiController.HacksDisplay_SetActive(true, selectedObject);
    }
    //Player Input END


    public void ChangeCamera(CameraObject cameraObject, bool pushToStack = true)
    {
        // if (!cameraObject.IsLocked)
        // {
        //     currentCamera = cameraManager.ChangeCamera(cameraObject, currentCamera);
        // }
        // else
        // {
        //     Debug.Log($"{cameraObject} is locked.");
        // }

        if (pushToStack&&cameraStackIndex > 0)
        {
            AddCurrentCameraToStack();
        }
        currentCamera = cameraManager.ChangeCamera(cameraObject, currentCamera);
        
        
        if (pushToStack)
        {
            AddCurrentCameraToStack();
        }

        if (currentCamera.IsLocked)
        {
            ActivateLockout(currentCamera);
        }
    }

    private void AddCurrentCameraToStack()
    {
        if (cameraStack.Contains(currentCamera))
        {
            cameraStack.Remove(currentCamera);
        }

        cameraStack.Insert(0, currentCamera);
        cameraStackIndex = 0;
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
        bool detectedSOHit = false;
        RaycastHit hit;

        //Finding Smart Objects, excluding cameras
        if (Physics.Raycast(currentCamera.Position, currentCamera.Forward, out hit, camera_CastRange, selectorLayer))
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

                detectedSOHit = true;
                Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.green,
                    Time.deltaTime);
            }
        }

        if (!detectedSOHit)
        {
            //Finding cameras
            if (Physics.Raycast(currentCamera.Position, currentCamera.Forward, out hit, camera_CastRange, cameraLayer))
            {
                CameraObject cameraObject = hit.collider.GetComponentInParent<CameraObject>();
                if (cameraObject)
                {
                    if (!cameraObject.Equals(selectedObject))
                    {
                        if (selectedObject)
                        {
                            selectedObject.OnSelect_Exit();
                        }

                        selectedObject = cameraObject;
                        selectedObject.OnSelect_Enter();
                    }

                    detectedSOHit = true;

                    Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.yellow,
                        Time.deltaTime);
                }
            }
        }

        if (!detectedSOHit)
        {
            //TODO - might need to check if it is worth disabling the hack wheel when it can't detect it no more
            if (selectedObject)
            {
                selectedObject.OnSelect_Exit();
                selectedObject = null;
            }

            Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.red, Time.deltaTime);
        }
    }

    public void ActivateLockout(CameraObject cameraObject)
    {
        uiController.LockoutScreen_SetActive(true, cameraObject);
        OnSelectCancel();
        cameraMode = CameraMode.LockedOut;
    }

    public void DeactivateLockout()
    {
        uiController.LockoutScreen_SetActive(false);
        cameraMode = CameraMode.Free;
    }
}