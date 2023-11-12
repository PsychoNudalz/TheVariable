using System;
using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        // SetActive(RoomLabel.None);
    }

    // Update is called once per frame
    void Update()
    {
        
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
