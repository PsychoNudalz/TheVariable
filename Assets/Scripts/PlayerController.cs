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

    private ControlMode controlMode = ControlMode.MK;

    [Header("Components")]
    [SerializeField]
    private CameraObject currentCamera;

    [Header("Settings")]
    [SerializeField]
    private float rotateMultiplier = 1f;

    [SerializeField]
    private float rotateMultiplier_joystick = 3f;

    private Vector2 lookValue;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        print("Joystick move: " + lookValue);
    }

    public void UpdateCamera(Vector2 rotation)
    {
        if (currentCamera)
        {
            currentCamera.RotateCamera(rotation.x, rotation.y);
        }
    }
}