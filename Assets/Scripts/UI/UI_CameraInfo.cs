using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CameraInfo : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField]
    private TextMeshProUGUI camera;
    [Header("Location")]
    [SerializeField]
    private TextMeshProUGUI location;
    [Header("Alert")]
    [SerializeField]
    private TextMeshProUGUI alert;

    private CameraController currentCamera = null;
    private NpcObject currentNPC = null;


    private void FixedUpdate()
    {
        if (currentCamera && currentCamera.IsNPC && currentNPC)
        {
            UpdateAlert(currentNPC);
        }
        
    }

    public void SetCamera(CameraController cameraController)
    {
        currentCamera = null;
        currentNPC = null;
        camera.text = cameraController.name.ToUpper();
        switch (cameraController.RoomLocation)
        {
            case RoomLabel.None:
                location.text = "N/A";
                break;
            case RoomLabel.LivingRoom:
                location.text = "Living_Room".ToUpper();

                break;
            case RoomLabel.Garage:
                location.text = "Garage".ToUpper();

                break;
            case RoomLabel.StaffToilet:
                location.text = "Toilet_Staff".ToUpper();

                break;
            case RoomLabel.Storage:
                location.text = "Storage_Room".ToUpper();

                break;
            case RoomLabel.ServantRoom:
                location.text = "Servant_Living_Quarters".ToUpper();

                break;
            case RoomLabel.GuardRoom:
                location.text = "Guard_Living_Quarters".ToUpper();

                break;
            case RoomLabel.Study:
                location.text = "Study_Room".ToUpper();

                break;
            case RoomLabel.MasterBedroom:
                location.text = "Master_Bedroom".ToUpper();

                break;
            case RoomLabel.Corridor:
                location.text = "Corridor".ToUpper();

                break;
            case RoomLabel.Connector:
                location.text = "Connector".ToUpper();

                break;
            case RoomLabel.Kitchen:
                location.text = "Kitchen".ToUpper();

                break;
            default:
                location.text = "N/A".ToUpper();
                break;
        }

        if (cameraController.ConnectedSo is NpcObject npcObject)
        {
            currentNPC = npcObject;
            UpdateAlert(npcObject);
        }
        else
        {
            alert.text = "N/A";

        }

        currentCamera = cameraController;
    }

    private void UpdateAlert(NpcObject npcObject)
    {
        if (!npcObject||!npcObject.Controller)
        {
            return;
        }
        alert.text = $"{npcObject.Controller.blackboardAlertState.ToString()}_{npcObject.Controller.AlertValueString}";
    }
}
