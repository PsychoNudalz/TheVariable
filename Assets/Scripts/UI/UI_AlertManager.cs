using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// New alert manager to update alert
/// </summary>
public class UI_AlertManager : MonoBehaviour
{
    [SerializeField]
    private UI_Alert uiAlertGo;

    [SerializeField]
    private Dictionary<NpcController, UI_Alert> npcToAlertMap;

    private Camera camera;

    [SerializeField]
    private Vector2 canvasSize = new Vector2(1280, 720);

    private void Awake()
    {
        camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!camera)
        {
            camera = Camera.main;
        }

        InitialiseMapping();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitialiseMapping()
    {
        npcToAlertMap = new Dictionary<NpcController, UI_Alert>();
        foreach (NpcController npcController in NpcManager.NPCs)
        {
            UI_Alert alert = Instantiate(uiAlertGo, transform).GetComponent<UI_Alert>();
            npcToAlertMap.Add(npcController, alert);
            alert.SetCanvasSize(canvasSize);
        }
    }

    public void UpdateAlert(NpcController npc, float value)
    {
        Vector2 uiCanvasPosition = Vector2.zero;

        if (value > 0)
        {
            uiCanvasPosition = camera.WorldToScreenPoint(npc.AlertPosition); 
        }

        npcToAlertMap[npc].SetAlert(uiCanvasPosition, value);
    }
}