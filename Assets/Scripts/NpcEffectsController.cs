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
    Sit,
    Eat,
    LayDown,
    Guard,
    Dead,
    Suspicious_Start,
}

/// <summary>
/// Handles visuals and audio effects for NPC
/// 
/// </summary>
public class NpcEffectsController : MonoBehaviour
{
    private bool isInAnimation = false;
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

    [SerializeField]
    private Vector3 originalPosition;

    [SerializeField]
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
        if (isInAnimation)
        {

            UpdateSecondary_GO();
            UpdateMaterial();

        }
    }

    private void FixedUpdate()
    {
    }

    void PlayAnimation(NpcAnimation npcAnimation)
    {
        // string newAnimation = "";
        switch (npcAnimation)
        {
            case NpcAnimation.None:
                break;
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

                PlayAnimator("NPC_Sus");
                break;
            case NpcAnimation.Spotted:
                if (sfx_Spotted)
                {
                    sfx_Spotted.Play();
                }

                PlayAnimator("NPC_Spotted");

                break;

            case NpcAnimation.Sit:
                PlayAnimator("NPC_Sit");
                break;
            case NpcAnimation.Eat:
                PlayAnimator("NPC_Eat");
                break;
            case NpcAnimation.LayDown:
                PlayAnimator("NPC_LayDown");
                break;
            case NpcAnimation.Guard:
                PlayAnimator("NPC_Guard");
                break;
            case NpcAnimation.Suspicious_Start:
                if (sfx_Spotted)
                {
                    sfx_Spotted.Play();
                }

                PlayAnimator("NPC_Spotted");

                break;
            default:
                PlayAnimator("NPC_Idle");
                break;
        }
        // Debug.Log($"{name} change animation: {currentAnimation} -> {npcAnimation}");
        currentAnimation = npcAnimation;
        animationTime_Current = 0;
        UpdateMaterial();
        isInAnimation = true;
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
            secondary_GO.SetActive(true);
            secondary_Animator.Play(currentAnimationString);
        }

        LockSecondary_GO_Transform();

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
        // if(currentAnimation.Equals(animation))
        // {
        //     Debug.Log($"{name} same animation: {currentAnimation}");
        //
        // }
        //Void the call to change animation if it was called with the same animation at a short distance
        float distance = Vector3.Distance(transform.position,position);
        float angle = Quaternion.Angle(transform.rotation, rotation);
        if (currentAnimation.Equals(animation) && (distance < 1f) && angle < 2f)
        {
            return;
        }
        if (!isInAnimation||(isInAnimation&&animationTime_Current>.5f)||(Vector3.Distance(originalPosition,transform.position)>2f))
        {
            originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            originalRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,
                transform.rotation.w);
            // originalRotation = transform.localToWorldMatrix * originalRotation;
        }

        transform.position = position;
        transform.rotation = rotation;
        LockSecondary_GO_Transform();

        if (animation == NpcAnimation.None)
        {
            animation = currentAnimation;
        }

        PlayAnimation(animation);
    }

    private void LockSecondary_GO_Transform()
    {
        secondary_GO.transform.position = originalPosition;
        secondary_GO.transform.rotation = originalRotation;
    }

    void UpdateMaterial()
    {
        if (animationTime_Current > 1f)
        {
            isInAnimation = false;
            secondary_Renderer.enabled = false;
            secondary_GO.SetActive(false);
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            LockSecondary_GO_Transform();
        }

        else
        {
            animationTime_Current += Time.deltaTime / animationTime;
            float offset = 1.1f;
            foreach (Material material in materials)
            {
                material.SetFloat("_MoveValue", animationTime_Current*offset);
            }

            foreach (Material material in secondary_Materials)
            {
                material.SetFloat("_MoveValue", animationTime_Current);
            }
        }
    }

    void UpdateSecondary_GO()
    {
        if (secondary_GO.activeSelf)
        {
            LockSecondary_GO_Transform();
        }
    }

    public void PlaySound_Sus()
    {
    }
}