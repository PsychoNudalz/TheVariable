using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smart object that contains a camera 
/// </summary>
[RequireComponent(typeof(CameraController) )]
public class CameraObject : SmartObject
{
    [SerializeField]
    private CameraController cameraController;
    public override Vector3 Position => cameraController.Position;
    public override Vector3 Forward => cameraController.Forward;

    public override Vector3 ColliderPosition => cameraController.ColliderPosition;

    public CameraController CameraController => cameraController;
    public bool IsPlayerControl => cameraController.IsPlayerControl;

    public bool IsHacking => cameraController.IsHacking;
    public bool IsLocked => cameraController.IsLocked;

    public bool IsDetectable => cameraController.IsDetectable;

    public float CameraLockTime => cameraController.CameraLockTime;
    public string CameraLockTime_String => cameraController.CameraLockTime_String;
    protected override void AwakeBehaviour()
    {
        if (!cameraController)
        {
            cameraController = GetComponent<CameraController>();
        }
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
    
}
