using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Objects that Both player and npc can interact with and investigate 
/// </summary>
public abstract class SmartObjects : MonoBehaviour
{
    [SerializeField]
    protected Transform interactPoint;

    public Vector3 Position => transform.position;
    public Vector3 InteractPosition => InteractPointPosition();

    protected virtual Vector3 InteractPointPosition()
    {
        if (!interactPoint)
        {
            return transform.position;
        }
        return interactPoint.position;
    }

    private void Awake()
    {
        AwakeBehaviour();
    }

    private void Start()
    {
        StartBehaviour();
    }

    private void Update()
    {
        UpdateBehaviour();
    }

    protected abstract void AwakeBehaviour();
    // Start is called before the first frame update
    protected abstract void StartBehaviour();

    // Update is called once per frame
    protected abstract void UpdateBehaviour();
    public abstract void Interact(NpcController npc);

}
