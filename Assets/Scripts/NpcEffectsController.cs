using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcAnimation
{
    Idle,
    Walk,
    Interact,
    PickUp,
    Suspicious,
    Dead
}

/// <summary>
/// Handles visuals and audio effects for NPC
/// 
/// </summary>
public class NpcEffectsController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Header("Material  Effect")]
    [SerializeField]
    private Renderer renderer;

    private Material[] materials;

    [SerializeField]
    private float animationTime = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float animationTime_Current = .5f;

    [Header("Sounds")]
    [SerializeField]
    private SoundAbstract sfx_Kneel;

    [SerializeField]
    private SoundAbstract sfx_Suspicious;

    private void Awake()
    {
        materials = renderer.materials;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayAnimation(NpcAnimation.Idle);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        UpdateMaterial();
    }

    public void PlayAnimation(NpcAnimation npcAnimation)
    {
        switch (npcAnimation)
        {
            case NpcAnimation.Idle:
                animator.Play("NPC_Idle");
                break;
            case NpcAnimation.Walk:
                animator.Play("NPC_Walk");

                break;
            case NpcAnimation.Interact:
                animator.Play("NPC_Interact");

                break;
            case NpcAnimation.PickUp:
                if (sfx_Kneel)
                {
                    sfx_Kneel.PlayF();
                }

                animator.Play("NPC_PickUp");

                break;
            case NpcAnimation.Dead:
                animator.Play("NPC_Dead");

                break;
            case NpcAnimation.Suspicious:
                if (sfx_Suspicious)
                {
                    sfx_Suspicious.Play();
                }
                break;
            default:
                animator.Play("NPC_Idle");
                break;
        }

        animationTime_Current = 0;
        // if (!(animationTime_Current > .2f && animationTime_Current < .9f))
        // {
        // }
    }

    public void UpdateMaterial()
    {
        if (animationTime_Current > 1)
        {
            return;
        }

        animationTime_Current += Time.deltaTime / animationTime;
        foreach (Material material in materials)
        {
            material.SetFloat("_MoveValue", animationTime_Current);
        }
    }

    public void PlaySound_Sus()
    {
    }
}