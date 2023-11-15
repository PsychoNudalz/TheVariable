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

    [Serializable]
    class GameObjectMapSpritePair
    {
        [SerializeField]
        private Transform transform;

        [SerializeField]
        private RectTransform sprite;

        public Transform Transform => transform;

        public Vector2 position => new Vector2(transform.position.x, transform.position.z);

        public RectTransform Sprite => sprite;

        public bool IsActive => sprite.gameObject.activeSelf;

        public void SetActive(bool b)
        {
            sprite.gameObject.SetActive(b);
        }

        public bool Equals(GameObject obj)
        {
            return transform.gameObject.Equals(obj);
        }

        public bool Equals(string str)
        {
            return sprite.name.ToUpper().Contains(str.ToUpper());
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

    [Header("Game Object Pairs")]
    [SerializeField]
    private GameObjectMapSpritePair[] gameObjectMapSpritePairs;

    private Transform mainCamera;

    private Vector2 camera2D => new Vector2(mainCamera.position.x, mainCamera.position.z);


    // Start is called before the first frame update
    void Start()
    {
        // SetActive(RoomLabel.None);
        mainCamera = Camera.main?.transform;
        UpdatePairs();
        SetActive_MapPair("Ex", false);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        UpdateCamera();
        UpdatePairs();
    }

    [ContextMenu("UpdateCamera")]
    private void UpdateCamera()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main?.transform;
        }

        UpdateMapIcon(camera2D, cameraSprite);
    }

    private void UpdateMapIcon(Vector2 worldPosition, RectTransform sprite)
    {
        sprite.localPosition = worldPosition * scale + offset;
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

    void UpdatePairs()
    {
        foreach (GameObjectMapSpritePair mapSpritePair in gameObjectMapSpritePairs)
        {
            if (mapSpritePair.IsActive&&mapSpritePair.Transform)
            {
                UpdateMapIcon(mapSpritePair.position, mapSpritePair.Sprite);
            }
        }
    }

    public void SetActive_MapPair(GameObject go, bool b)
    {
        foreach (GameObjectMapSpritePair mapSpritePair in gameObjectMapSpritePairs)
        {
            if (mapSpritePair.Equals(go))
            {
                mapSpritePair.SetActive(b);
            }
        }
    }

    public void SetActive_MapPair(string str, bool b)
    {
        foreach (GameObjectMapSpritePair mapSpritePair in gameObjectMapSpritePairs)
        {
            if (mapSpritePair.Equals(str))
            {
                mapSpritePair.SetActive(b);
            }
        }
    }

    public void ShowExtraction()
    {
        SetActive_MapPair("VIP", false);
        SetActive_MapPair("Ex", true);
    }
}