using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_LockOutScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private CameraObject connectedCamera;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (connectedCamera)
        {
            text.text = GetCameraTime();
            if (!connectedCamera.IsLocked)
            {
                SetActive(false);
            }
        }
    }

    string GetCameraTime()
    {
        return connectedCamera.CameraLockTime_String;
    }

    public void SetActive(bool b, CameraObject cameraObject =null)
    {
        if (gameObject.activeSelf == b)
        {
            return;
        }
        gameObject.SetActive(b);
        connectedCamera = cameraObject;
    }
}
