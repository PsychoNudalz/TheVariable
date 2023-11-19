using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using HighlightPlus;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;


public enum CameraInvestigationMode
{
    None,
    Investigated,
    Spotted
}

/// <summary>
/// Handles the controls and behaviour of a camera
/// Allows NPC to detect and Investigate this
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private RoomLabel roomLocation = RoomLabel.None;

    [Header("Movement")]
    [SerializeField]
    private CinemachineVirtualCamera camera;

    [SerializeField]
    private Transform camera_transform;

    [SerializeField]
    private Vector2 xClamp = new Vector2(-90f, 90f);

    [SerializeField]
    private Vector2 yClamp = new Vector2(-120f, 120f);

    [Header("Zoom")]
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

    [SerializeField]
    private Renderer cameraRenderer;

    [SerializeField]
    private Material lockedOutMaterial;

    [SerializeField]
    private Renderer cameraRingRenderer;

    [SerializeField]
    private Material cameraRingMaterial;

    private Material[] originalMaterials;

    [SerializeField] public bool IsNPC => (connectedSO && connectedSO is NpcObject);

    [Serializable]
    enum CameraState
    {
        None,
        Hacking,
        Locked
    }

    [Header("Hacking")]
    [SerializeField]
    private CameraState cameraState = CameraState.None;

    private SmartObject savedHack_SO;
    private int savedHack_index = 0;
    private HackContext_Enum[] savedHack_ContextEnum;
    // private Coroutine hackCoroutine;

    [SerializeField]
    private LineRenderer hackLine;
    // private LineRenderer hackLine_Material;

    // [SerializeField]
    private Transform hackTarget;
    private float cameraHack_Time = 0;
    private float cameraHack_TimeLeft = 0;

    [Header("Locking")]
    [SerializeField]
    private CameraInvestigationMode investigationMode;

    private float cameraLock_Time = 0;

    [Space(10)]
    [Header("Components")]
    [SerializeField]
    private SmartObject connectedSO;

    [SerializeField]
    private Renderer cameraBody;

    [SerializeField]
    private Collider disableCollider;

    [SerializeField]
    Collider mainCollider;


    PlayerController playerController;

    public SmartObject ConnectedSo => connectedSO;

    private Vector3 cameraOrientation = default;

    private const int CameraPriority = 10;

    private bool isPlayerControl = false;
    private UIController uiController;
    private float lastInvestigateTime;
    private float investigateFailSafeTime = 5f;
    private float maxHackSpeedUp = .15f;
    public Vector3 Position => camera_transform.position;
    public Vector3 Forward => camera_transform.forward;

    public Vector3 ColliderPosition => mainCollider.transform.position;
    public Vector3 InteractPosition => connectedSO.InteractPosition;
    public Quaternion InteractRotation => connectedSO.InteractRotation;

    public bool IsPlayerControl => isPlayerControl;

    public bool IsHacking => cameraState == CameraState.Hacking;
    public bool IsLocked => cameraState == CameraState.Locked;

    public bool IsDetectable => !IsLocked && (IsPlayerControl || IsHacking);

    public float CameraLockTime => cameraLock_Time;
    public string CameraLockTime_String => Get_CameraLockTime_String();

    public RoomLabel RoomLocation => roomLocation;

    void Awake()
    {
        cameraOrientation = camera_transform.eulerAngles;
        originalFOV = camera.m_Lens.FieldOfView;
        uiController = UIController.current;

        SetActive(false);
        hackLine.gameObject.SetActive(false);
        if (cameraRenderer)
        {
            originalMaterials = cameraRenderer.materials;
        }

        if (cameraRingRenderer)
        {
            cameraRingMaterial = cameraRingRenderer.material;
        }

        if (!connectedSO)
        {
            connectedSO = GetComponentInParent<SmartObject>();
        }
    }

    void Start()
    {
        playerController = PlayerController.current;
        SetCameraRotation(0, 0);
    }

    void Update()
    {
        if (throughWallEffect_Active)
        {
            ThroughWallEffect_Update();
        }

        switch (cameraState)
        {
            case CameraState.None:
                break;
            case CameraState.Hacking:
                if (hackLine && hackTarget)
                {
                    if (cameraHack_TimeLeft > 0)
                    {
                        cameraHack_TimeLeft -= Time.deltaTime;
                    }

                    HackLine_Update();

                    if (cameraHack_TimeLeft < 0)
                    {
                        Hack_Activation(savedHack_SO,savedHack_index,savedHack_ContextEnum);
                    }
                }

                break;
            case CameraState.Locked:
                if (cameraLock_Time > 0)
                {
                    cameraLock_Time -= Time.deltaTime;
                }
                else
                {
                    Set_Lock(false);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FixedUpdate()
    {
        if (investigationMode == CameraInvestigationMode.Investigated)
        {
            CameraRevertToNormalFailSafe();
        }
    }

    private void HackLine_Update()
    {
        hackLine.SetPosition(0, Position + new Vector3(0, -.2f, 0));
        if (!hackTarget)
        {
            return;
        }

        hackLine.SetPosition(1, hackTarget.position);
        hackLine.material.SetVector("_Effect_Animation_Position", hackLine.transform.position);
        hackLine.material.SetFloat("_Effect_Animation_Distance",
            (Position - hackTarget.position).magnitude);
        hackLine.material.SetFloat("_Effect_Animation_Value", 1 - (cameraHack_TimeLeft / cameraHack_Time));
    }

    void HackLine_Reset()
    {

        hackLine.gameObject.SetActive(false);
        HackLine_Update();
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
        cameraOrientation.y = Mathf.Clamp(cameraOrientation.y, yClamp.x, yClamp.y);
        camera_transform.localEulerAngles = cameraOrientation;
    }

    void SetCameraRotation(float horizontal, float vertical)
    {
        cameraOrientation.x = vertical;
        cameraOrientation.y = horizontal;
        camera_transform.localEulerAngles = cameraOrientation;
    }

    public void UpdateZoom(float zoomAmount)
    {
        zoomLevel = Mathf.Clamp(zoomAmount * Time.deltaTime + zoomLevel, 0f, 1f);
        camera.m_Lens.FieldOfView = Mathf.Lerp(originalFOV, originalFOV * zoomFOVMultiplier, zoomLevel);
    }

    public void SetActive(bool b)
    {
        if (!uiController)
        {
            uiController = UIController.current;
        }

        if (b)
        {
            camera.Priority = CameraPriority;

            Invoke(nameof(DelayDisableCollider), .5f);


            if (cameraRingRenderer)
            {
                cameraRingRenderer.gameObject.SetActive(false);
            }

            uiController.Minimap_Active(roomLocation);
        }
        else
        {
            camera.Priority = -1;
            Invoke(nameof(DelayEnableCollider), .5f);


            if (cameraRingRenderer)
            {
                cameraRingRenderer.gameObject.SetActive(true);
            }
        }

        isPlayerControl = b;
        SetInvestigationMode(investigationMode);
        uiController.SetCamera(this);
    }

    private void DelayEnableCollider()
    {
        if (disableCollider)
        {
            disableCollider.enabled = true;
        }

        if (cameraBody)
        {
            cameraBody.enabled = true;
        }
    }

    private void DelayDisableCollider()
    {
        if (disableCollider)
        {
            disableCollider.enabled = false;
        }

        if (cameraBody)
        {
            cameraBody.enabled = false;
        }
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

    /// <summary>
    /// Starting the hack
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index">index of the hack on the target</param>
    /// <param name="hackContextEnum"></param>
    public void StartHack(SmartObject target, int index, HackContext_Enum[] hackContextEnum = default)
    {
        // if (cameraState != CameraState.None)
        // {
        //     Debug.Log($"{name} state not none");
        //     return;
        // }
        CancelHack();

        if (index < 0)
        {
            Debug.LogError($"{name} hack on {target.name} index < 0");
            return;
        }

        if (!target.Hacks[index].CanHack())
        {
            Debug.LogError($"{name} hack on {target.name} can NOT hack");

            return;
        }

        // hackCoroutine = StartCoroutine(HackRoutine(target, index, hackContextEnum));
        
        Hack_Initialise(target, index);
        savedHack_ContextEnum = hackContextEnum;
        UIController.current.HackSpeedUp_SetActive(true);

    }

    // IEnumerator HackRoutine(SmartObject target, int index, HackContext_Enum[] hackContextEnum = default)
    // {
    //     Hack_Initialise(target, index);
    //     yield return new WaitForSeconds(target.Hacks[index].HackTime);
    //     Hack_Activation(target, index, hackContextEnum);
    // }

    /// <summary>
    /// Saves the values for the hack
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index"></param>
    private void Hack_Initialise(SmartObject target, int index)
    {
        cameraState = CameraState.Hacking;
        hackLine.gameObject.SetActive(true);
        hackTarget = target.transform;
        float time = target.Hacks[index].HackTime;
        cameraHack_Time = time;
        cameraHack_TimeLeft = time;
        SoundManager.PlayGlobal(SoundGlobal.Hacking);
        
        savedHack_SO = target;
        savedHack_index = index;
    }


    /// <summary>
    /// activate the hack and apply the hack effects etc
    /// </summary>
    /// <param name="target"></param>
    /// <param name="index"></param>
    /// <param name="hackContextEnum"></param>
    private void Hack_Activation(SmartObject target, int index, HackContext_Enum[] hackContextEnum = default)
    {
        target.ActivateHack(index, hackContextEnum);


        // hackLine.gameObject.SetActive(false);
        // HackLine_Reset();
        Hack_End();
        SoundManager.PlayGlobal(SoundGlobal.HackComplete);
    }

    public void Set_Lock(bool b, float duration = 0f)
    {
        Set_LockMaterial(b);
        if (b)
        {
            // HackLine_Reset();
            CancelHack();

            cameraState = CameraState.Locked;
            if (cameraLock_Time <= 0)
            {
                cameraLock_Time = duration;
            }

            if (isPlayerControl)
            {
                playerController.ActivateLockout(this);
                if (GameManager.RanOutOfTime)
                {
                    GameManager.GameOver();
                }
                //Changes to the starting camera
            }
            //Return to previous camera if current camera is an NPC camera
            //Comment: Not too sure if I should do that or just reset it to the starting camera
            // if (playerController)
            // {
            // }
        }
        else
        {
            cameraState = CameraState.None;
            playerController.DeactivateLockout();
            if (isPlayerControl)
            {
                playerController.ChangeCamera(CameraManager.current.StartingCamera);
            }

            SetInvestigationMode(CameraInvestigationMode.None);
        }
    }
    

    public void Set_Investigate(bool b)
    {
        if (investigationMode == CameraInvestigationMode.Spotted)
        {
            // Debug.Log($"{name} is in spotted");
            return;
        }

        if (b)
        {
            SetInvestigationMode(CameraInvestigationMode.Investigated);
        }
        else
        {
            SetInvestigationMode(CameraInvestigationMode.None);
        }
    }


    public void CameraRevertToNormalFailSafe()
    {
        if (Time.time - lastInvestigateTime > investigateFailSafeTime)
        {
            Debug.LogWarning($"{name} camera investigate failsafe triggered");
            Set_Investigate(false);
        }
    }
    
    public void SetInvestigationMode(CameraInvestigationMode mode)
    {
        if (mode == CameraInvestigationMode.Investigated)
        {
            lastInvestigateTime = Time.time;
        }
        investigationMode = mode;
        if (isPlayerControl)
        {
            uiController.CameraScreen_Play(mode);
        }

        if (cameraRingMaterial)
        {
            cameraRingMaterial.SetInt("_CameraRingMode", (int)mode);
        }
    }

    /// <summary>
    /// for finishing and reseting hacking values
    /// </summary>
    void Hack_End()
    {
        if (cameraState != CameraState.Locked &&
            investigationMode != CameraInvestigationMode.Spotted)
        {
            //forces the camera to still be hacking if it is spotted to avoid NPCs from changing it's investigation target
            cameraState = CameraState.None;
        }
        hackTarget = null;
        cameraHack_TimeLeft = 0;
        cameraHack_Time = 0;
        SoundManager.StopGlobal(SoundGlobal.Hacking);
        UIController.current.HackSpeedUp_SetActive(false);
        HackLine_Reset();

    }

    public void CancelHack()
    {
        // if (hackCoroutine != null)
        // {
        //     //Stopping the hack
        //     StopCoroutine(hackCoroutine);
        //     hackCoroutine = null;
        //     SoundManager.PlayGlobal(SoundGlobal.HackStop);
        // }
        if (cameraState == CameraState.Hacking&&isPlayerControl)
        {
            SoundManager.PlayGlobal(SoundGlobal.HackStop);
        }
        Hack_End();
        
    }

    public void SpeedHack(float speedUp)
    {
        cameraHack_TimeLeft -= Mathf.Min(speedUp,cameraHack_Time*maxHackSpeedUp);
    }
    

    void Set_LockMaterial(bool b)
    {
        if (!cameraRenderer)
        {
            return;
        }

        if (b)
        {
            for (var i = 0; i < cameraRenderer.materials.Length; i++)
            {
                cameraRenderer.materials[i] = lockedOutMaterial;
            }
        }
        else
        {
            for (var i = 0; i < cameraRenderer.materials.Length; i++)
            {
                cameraRenderer.materials[i] = originalMaterials[i];
            }
        }
    }

    String Get_CameraLockTime_String()
    {
        double seconds = Math.Floor((cameraLock_Time % 1f) * 100);
        return string.Concat(cameraLock_Time.ToString("0"), ":", seconds.ToString("0"));
    }

    public void Override_IsHacking()
    {
        if (!IsHacking)
        {
            cameraState = CameraState.Hacking;
        }
    }
}