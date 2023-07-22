using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;

/// <summary>
/// Objects that Both player and npc can interact with and investigate 
/// </summary>
[RequireComponent(typeof(HighlightEffect))]
public abstract class SmartObject : MonoBehaviour
{
    [SerializeField]
    protected Transform interactPoint;

    private HighlightEffect highlightEffect;
    public Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
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
        highlightEffect = GetComponent<HighlightEffect>();
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

    public virtual void OnSelect_Enter()
    {
        highlightEffect.highlighted = true;
    }

    public virtual void OnSelect_Exit()
    {
        highlightEffect.highlighted = false;
    }
}