using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.UI;


public class UI_Minimap : MonoBehaviour
{
    [Serializable]
    class MinimapPair
    {
        [SerializeField]
        private RoomLabel room;
        [SerializeField]
        private GameObject mapSprite;


        public void SetActive(bool b)
        {
            mapSprite.SetActive(b);
        }
        public bool Equals(RoomLabel r)
        {
            return r.Equals(room);
        }
        public override bool Equals(object obj)
        {
            if (obj is RoomLabel r)
            {
                return r.Equals(room);
            }
            return base.Equals(obj);
        }
    }

    [SerializeField]
    private MinimapPair[] minimapPairs;
    
    [Header("Camera")]
    [SerializeField]
    private RectTransform cameraSprite;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private float scale = 1f;

    private Transform mainCamera;

    private Vector2 camera2D => new Vector2(mainCamera.position.x, mainCamera.position.z);


    // Start is called before the first frame update
    void Start()
    {
        // SetActive(RoomLabel.None);
        mainCamera = Camera.main?.transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    [ContextMenu("UpdateCamera")]
    private void UpdateCamera()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main?.transform;
        }
        cameraSprite.localPosition = camera2D * scale + offset;
    }

    public void SetActive(RoomLabel roomLabel)
    {
        foreach (MinimapPair minimapPair in minimapPairs)
        {
            if (minimapPair.Equals(roomLabel))
            {
                minimapPair.SetActive(true);
            }

            else
            {
                minimapPair.SetActive(false);
            }
        }
    }
    
    
}
