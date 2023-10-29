using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NpcAnimation
{
    None,
    Idle,
    Walk,
    Interact,
    PickUp,
    Suspicious,
    Spotted,
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

    [SerializeField]
    private NavMeshAgent navMeshAgent;


    private NpcAnimation currentAnimation;
    private string currentAnimationString;

    [Header("Material Effect")]
    [Header("Animations")]
    [SerializeField]
    private Renderer renderer;

    private Material[] materials;

    [SerializeField]
    private float animationTime = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float animationTime_Current = .5f;

    [Header("Secondary")]
    [SerializeField]
    private GameObject secondary_GO;

    private Animator secondary_Animator;
    private Renderer secondary_Renderer;
    private Material[] secondary_Materials;

    [Header("Sounds")]
    [SerializeField]
    private SoundAbstract sfx_Kneel;

    [SerializeField]
    private SoundAbstract sfx_Suspicious;

    [SerializeField]
    private SoundAbstract sfx_Spotted;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        materials = renderer.materials;


        //Duplicate animator
        DuplicateAndSetSecondarySkin();
        if (!navMeshAgent)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    public void DuplicateAndSetSecondarySkin()
    {
        secondary_GO = Instantiate(animator.gameObject, animator.transform.parent);
        secondary_Animator = secondary_GO.GetComponent<Animator>();
        secondary_Renderer = secondary_GO.GetComponentInChildren<SkinnedMeshRenderer>();
        secondary_Materials = secondary_Renderer.materials;
        if (secondary_GO.TryGetComponent(out EffectPlayer effectPlayer))
        {
            effectPlayer.Reset();
        }

        foreach (Material secondaryMaterial in secondary_Materials)
        {
            secondaryMaterial.SetInt("_IsSecondary", 1);
        }
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
        // string newAnimation = "";
        switch (npcAnimation)
        {
            case NpcAnimation.Idle:
                PlayAnimator("NPC_Idle");
                break;
            case NpcAnimation.Walk:
                PlayAnimator("NPC_Walk");

                break;
            case NpcAnimation.Interact:
                PlayAnimator("NPC_Interact");

                break;
            case NpcAnimation.PickUp:
                if (sfx_Kneel)
                {
                    sfx_Kneel.PlayF();
                }

                PlayAnimator("NPC_PickUp");

                break;
            case NpcAnimation.Dead:
                PlayAnimator("NPC_Dead");

                break;
            case NpcAnimation.Suspicious:
                if (sfx_Suspicious)
                {
                    sfx_Suspicious.Play();
                }

                break;
            case NpcAnimation.Spotted:
                if (sfx_Spotted)
                {
                    sfx_Spotted.Play();
                }

                break;

            default:
                PlayAnimator("NPC_Idle");
                break;
        }

        currentAnimation = npcAnimation;
        animationTime_Current = 0;
        UpdateMaterial();
    }

    /// <summary>
    /// This sets the animation for the main and secondary animator
    /// </summary>
    /// <param name="stateName"></param>
    private void PlayAnimator(string stateName)
    {
        if (animationTime_Current > .9f)
        {
            secondary_Renderer.enabled = true;

            secondary_Animator.Play(currentAnimationString);
        }

        currentAnimationString = stateName;
        animator.Play(stateName);
    }

    /// <summary>
    ///  Move the Character's transform and play new animation, would default to it's current animation if none is supplied
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="animation"></param>
    public void MoveTransform(Vector3 position, Quaternion rotation, NpcAnimation animation)
    {
        navMeshAgent.enabled = false;
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        originalRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,
            transform.rotation.w);
        // originalRotation = transform.localToWorldMatrix * originalRotation;
        transform.position = position;
        transform.rotation = rotation;

        LockSecondary_GO_Transform();

        if (animation == NpcAnimation.None)
        {
            animation = currentAnimation;
        }

        PlayAnimation(animation);
        navMeshAgent.enabled = true;
    }

    private void LockSecondary_GO_Transform()
    {
        secondary_GO.transform.position = originalPosition;
        secondary_GO.transform.rotation = originalRotation;
    }

    public void UpdateMaterial()
    {
        if (animationTime_Current > 1)
        {
            secondary_Renderer.enabled = false;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        else
        {
            animationTime_Current += Time.deltaTime / animationTime;
            foreach (Material material in materials)
            {
                material.SetFloat("_MoveValue", animationTime_Current);
            }

            foreach (Material material in secondary_Materials)
            {
                material.SetFloat("_MoveValue", animationTime_Current);
            }
        }

        LockSecondary_GO_Transform();
    }

    public void PlaySound_Sus()
    {
    }
}