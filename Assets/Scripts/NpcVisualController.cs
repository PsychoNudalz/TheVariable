using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcAnimation
{
    Idle,
    Walk,
    Interact
}
public class NpcVisualController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        PlayAnimation(NpcAnimation.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            default:
                throw new ArgumentOutOfRangeException(nameof(npcAnimation), npcAnimation, null);
        }
    }
}
