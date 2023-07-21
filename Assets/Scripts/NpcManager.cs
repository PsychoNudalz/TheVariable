using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    //TODO: Add in NPC controller class
    [SerializeField]
    private List<NpcData> npcs;

    public static NpcManager current;
    public static List<NpcData> NPCs => current.npcs;
    private void Awake()
    {
        current = this;
        npcs = new List<NpcData>(FindObjectsOfType<NpcData>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
