using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    //TODO: Add in NPC controller class
    [SerializeField]
    private List<NpcController> npcs;

    public static NpcManager current;
    public static List<NpcController> NPCs => current.npcs;
    private void Awake()
    {
        current = this;
        npcs = new List<NpcController>(FindObjectsOfType<NpcController>());
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
