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
        LockedOut,
        StopInput
    }

    private ControlMode controlMode = ControlMode.MK;
    private ZoomMode zoomMode = ZoomMode.None;
    private CameraMode cameraMode = CameraMode.Free;
    private CameraMode originalCameraMode = CameraMode.Free;

    [Header("Components")]
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private CameraController currentCamera;

    [SerializeField]
    private List<CameraController> cameraStack = new List<CameraController>(6);

    private int cameraStackIndex = 0;
    CameraController currentCameraFromStack => cameraStack[cameraStackIndex];

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

    /// <summary>
    /// Flag to top the camera from looking unless the player let go of the direction button
    /// </summary>
    private bool hackingToLookingFlag = false;

    [Header("Selector")]
    [SerializeField]
    [Space(5)]
    private SmartObject selectedObject;

    [SerializeField]
    private LayerMask selectorLayer;

    [SerializeField]
    private bool detectCameraThroughWalls = false;

    [SerializeField]
    private LayerMask cameraLayer;

    [SerializeField]
    private float camera_CastRange = 20f;

    [SerializeField]
    private float selectStickyTime = 1f;

    private float selectStickyTime_Now = 1f;


    private Vector2 lookValue;

    [SerializeField]
    private Vector2 selectDir;

    public static PlayerController current;

    //This should be in a separate script but fk it
    [Header("Player Stats")]
    [SerializeField]
    private int clearanceLevel = 0;

    [SerializeField]
    private ItemName copiedItem = ItemName.None;

    [SerializeField]
    private int collectedGB = 0;

    [SerializeField]
    private float speedUpAmount = .1f;

    public int ClearanceLevel => clearanceLevel;

    public int CollectedGb => collectedGB;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraController cameraController = cameraManager.StartingCamera;
        if (cameraController)
        {
            ChangeCamera(cameraController);
        }
        else
        {
            ChangeCamera(cameraManager.AllCameras[0]);
        }

        CopyItem(ItemName.None);
        collectedGB = 0;
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

        switch (cameraMode)
        {
            case CameraMode.Free:
                break;
            case CameraMode.SelectHack:
                uiController.HacksDisplay_UpdateDir(selectDir);

                break;
            case CameraMode.LockedOut:
                break;
            case CameraMode.StopInput:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (cameraMode)
        {
            case CameraMode.Free:
                RaycastCamera();
                uiController.SOInfo_Hover(selectedObject);

                break;
            case CameraMode.SelectHack:
                break;
            case CameraMode.LockedOut:
                if (!currentCamera.IsLocked)
                {
                    DeactivateLockout();
                }

                break;
            case CameraMode.StopInput:
                break;
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
        Vector2 inputVector = inputValue.Get<Vector2>();

        if (inputVector.magnitude <= .2f)
        {
            hackingToLookingFlag = false;
        }

        lookValue = inputVector * rotateMultiplier_joystick;
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
        hackingToLookingFlag = true;
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

        if (cameraMode is CameraMode.SelectHack or CameraMode.LockedOut)
        {
            return;
        }


        if (!currentCamera.IsNPC)
        {
            if (cameraStackIndex == 0)
            {
                return;
            }

            cameraStackIndex--;
            cameraStackIndex = (int)Math.Clamp(cameraStackIndex, 0, cameraStack.Count - 1);
        }

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

        if (cameraMode is CameraMode.SelectHack or CameraMode.LockedOut)
        {
            return;
        }


        if (!currentCamera.IsNPC)
        {
            if (cameraStackIndex == cameraStack.Count - 1)
            {
                return;
            }

            cameraStackIndex++;
            cameraStackIndex = (int)Math.Clamp(cameraStackIndex, 0, cameraStack.Count - 1);
        }

        SwitchToStackCamera();

        // ChangeCamera(cameraStack[cameraStackIndex],false);
        // cameraStack[cameraStackIndex].ActivateHack<Hack_Camera_Switch>(new HackContext_Enum[1]
        // {
        //     HackContext_Enum.Camera_notPushToStack
        // });

        // currentCamera = cameraManager.GetPrevCamera(currentCamera);
    }

    public void OnWindow_Next(InputValue inputValue)
    {
        TutorialManager.current.OnWindow_Next();
    }

    public void OnWindow_Prev(InputValue inputValue)
    {
        TutorialManager.current.OnWindow_Prev();
    }

    public void OnWindow_Close(InputValue inputValue)
    {
        TutorialManager.current.OnWindow_Close();
    }

    public void LockInput(bool b)
    {
        if (b)
        {
            if (cameraMode == CameraMode.StopInput)
            {
                return;
            }

            originalCameraMode = cameraMode;
            cameraMode = CameraMode.StopInput;
        }
        else
        {
            cameraMode = originalCameraMode;
        }
    }


    private void SwitchToStackCamera()
    {
        // if (currentCamera.ConnectedSo is NpcObject)
        // {
        //     cameraStackIndex = 0;
        // }

        currentCamera.StartHack(currentCameraFromStack.ConnectedSo,
            currentCameraFromStack.ConnectedSo.GetHackIndex<Hack_Camera_Switch>(),
            new HackContext_Enum[1]
            {
                HackContext_Enum.Camera_notPushToStack
            });
        uiController.CameraStack_SetIndex(cameraStackIndex);
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
                CancelHack();
                break;
            case CameraMode.SelectHack:
                SelectHack(Vector2.zero);
                break;
            case CameraMode.LockedOut:
                break;
            case CameraMode.StopInput:
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

    public void OnToggleHUD(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            uiController.CycleHUD();
        }
    }

    public void OnResetLevel(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            GameManager.ResetLevel();
        }
    }

    public void OnResetSave(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            GameManager.ResetSave();
        }
    }

    public void OnHack_SpeedUp(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            currentCamera.SpeedHack(speedUpAmount);
        }
    }

    public void OverrideSpeedUp(float speed)
    {
        speedUpAmount = speed;
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
        GameManager.ResetTimeScale();
    }

    void CancelHack()
    {
        currentCamera.CancelHack();
    }


    private void DisplayHack()
    {
        cameraMode = CameraMode.SelectHack;
        selectDir = Vector2.zero;
        uiController.HacksDisplay_SetActive(true, selectedObject);
        GameManager.SlowTimeForHack();
    }
    //Player Input END


    public void ChangeCamera(CameraController cameraController, bool pushToStack = true)
    {
        // if (!CameraController.IsLocked)
        // {
        //     currentCamera = cameraManager.ChangeCamera(CameraController, currentCamera);
        // }
        // else
        // {
        //     Debug.Log($"{CameraController} is locked.");
        // }
        if (cameraController.ConnectedSo is NpcObject)
        {
            //NPC cameras does not get pushed to stack
            pushToStack = false;
        }

        if (pushToStack && cameraStackIndex > 0)
        {
            AddCurrentCameraToStack();
        }

        currentCamera = cameraManager.ChangeCamera(cameraController, currentCamera);


        if (pushToStack)
        {
            AddCurrentCameraToStack();
        }

        if (currentCamera.IsLocked)
        {
            ActivateLockout(currentCamera);
        }
        // if(currentCamera)
    }

    /// <summary>
    /// Adds current camera to stack
    /// </summary>
    private void AddCurrentCameraToStack()
    {
        if (cameraStack.Contains(currentCamera))
        {
            cameraStack.Remove(currentCamera);
        }

        cameraStack.Insert(0, currentCamera);
        cameraStackIndex = 0;
        while (cameraStack.Count > 6)
        {
            cameraStack.RemoveAt(6);
        }

        uiController.CameraStack_AddStack(cameraStack.ToArray(), cameraStackIndex);
    }

    void WipeCameraStack()
    {
        cameraStack = new List<CameraController>();
        cameraStackIndex = 0;
        uiController.CameraStack_AddStack(cameraStack.ToArray(), cameraStackIndex);
    }


    void UpdateCamera(Vector2 rotation)
    {
        if (hackingToLookingFlag)
        {
            return;
        }

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
            if (smartObject && smartObject.HasHacks())
            {
                selectStickyTime_Now = selectStickyTime;
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


        if (detectCameraThroughWalls)
        {
            if (!detectedSOHit)
            {
                //Finding cameras
                if (Physics.Raycast(currentCamera.Position, currentCamera.Forward, out hit, camera_CastRange,
                        cameraLayer))
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
        }

        if (!detectedSOHit)
        {
            if (selectedObject)
            {
                selectStickyTime_Now -= Time.deltaTime;
            }

            if (selectStickyTime_Now < 0f)
            {
                //TODO - might need to check if it is worth disabling the hack wheel when it can't detect it no more
                if (selectedObject)
                {
                    selectedObject.OnSelect_Exit();
                    selectedObject = null;
                }

                Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.red,
                    Time.deltaTime);
            }
            else if (selectedObject)
            {
                Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.blue,
                    Time.deltaTime);
                Debug.DrawLine(currentCamera.Position, selectedObject.Position, Color.cyan, Time.deltaTime);
            }
            else
            {
                Debug.DrawRay(currentCamera.Position, currentCamera.Forward * camera_CastRange, Color.red,
                    Time.deltaTime);
            }
        }
    }

    public void ActivateLockout(CameraController CameraController)
    {
        uiController.LockoutScreen_SetActive(true, CameraController);
        OnSelectCancel();
        cameraMode = CameraMode.LockedOut;
        WipeCameraStack();


        //Changed lockout to not reduce clearance
        // DecreaseClearanceLevel(1);
    }

    public void DeactivateLockout()
    {
        uiController.LockoutScreen_SetActive(false);
        cameraMode = CameraMode.Free;
    }

    public void SetJoystickMultiplier(float m)
    {
        rotateMultiplier_joystick = m;
    }


    //----------------------
    //Clearance Level
    public void IncreaseClearanceLevel(int level)
    {
        clearanceLevel = Math.Max(clearanceLevel, level);
        Debug.Log($"Player Level increased to: {clearanceLevel}");
        uiController.SetClearanceText(clearanceLevel);
        SoundManager.PlayGlobal(SoundGlobal.ClearanceLevel);
    }

    public void DecreaseClearanceLevel(int level)
    {
        clearanceLevel = Math.Max(clearanceLevel - level, 0);
        Debug.Log($"Player Level decreased to: {clearanceLevel}");
        uiController.SetClearanceText(clearanceLevel);
    }

    //Copy object
    public void CopyItem(ItemName itemName)
    {
        copiedItem = itemName;
        uiController.SetItem(itemName);
    }

    public void PasteItem(ItemObject itemObject)
    {
        // itemObject
        if (copiedItem == ItemName.None)
        {
            return;
        }

        itemObject.PasteItem(copiedItem);
    }

    //Collect GB
    public void AddGB(int gb)
    {
        collectedGB += gb;
        UIController.current.Objective_SetData(collectedGB);
    }

    public void OnExitGame()
    {
        Application.Quit();
        //Note: this might kill Richard's program so will need to test
        // System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}