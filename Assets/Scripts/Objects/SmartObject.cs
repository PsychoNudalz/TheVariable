using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

/// <summary>
/// Objects that Both player and npc can interact with and investigate 
/// </summary>
[RequireComponent(typeof(HighlightEffect))]
public abstract class SmartObject : MonoBehaviour
{
    [Header("Smart Object")]
    [SerializeField]
    protected Transform interactPoint;

    [SerializeField]
    private HighlightEffect highlightEffect;

    [Header("Hacks")]
    [SerializeReference]
    private HackAbility[] hacks = Array.Empty<HackAbility>();

    [SerializeField]
    private Transform hackPoint;
    [SerializeField]
    private UnityEvent onHackSelectEvent;

    [SerializeField]
    private UnityEvent onHackActivateEvent;

    [Header("Audio Distract")]
    [SerializeField]
    private SO_AudioDistraction audioDistract;
    
    
    //This should have been on a separate effects controller but I really CBA to add one now
    [Header("Hack Change Effect")]
    [SerializeField]
    protected VisualEffect dataEffect;
    [SerializeField]
    protected MeshRenderer[] renderers;
    [SerializeField]
    protected Material goldlessMaterial;


    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
    public virtual Vector3 ColliderPosition => transform.position;
    public Transform InteractTransform => InteractPointTransform();
    public Vector3 InteractPosition => InteractPointPosition();
    public Quaternion InteractRotation => InteractPointTransform().rotation;

    public virtual Vector3 HackPosition => GetHackPosition();



    public HackAbility[] Hacks => hacks;


    // [Header("Tutorial On Hack")]
    // [SerializeField]
    // private TutorialEnum tutorialOnHack = TutorialEnum.HackingControls;
    [Header("Debug")]
    [SerializeField]
    private bool Enable_Debug = true;



    public bool HasHacks()
    {
        foreach (HackAbility hackAbility in hacks)
        {
            if (hackAbility.ShowHack)
            {
                return true;
            }
        }
        return false;

    }
    private void OnDrawGizmos()
    {
        if (Enable_Debug)
        {
            Gizmos.color = new Color(.5f, .5f, .5f, .5f);
            if (interactPoint)
            {
                Gizmos.DrawCube(InteractPosition + new Vector3(0f, .1f, 0f), new Vector3(.25f, .25f, .25f));
            }
        }
    }

    protected virtual Vector3 InteractPointPosition()
    {
        if (!interactPoint)
        {
            return transform.position;
        }

        return interactPoint.position;
    }

    protected virtual Transform InteractPointTransform()
    {
        if (!interactPoint)
        {
            return transform;
        }

        return interactPoint;
    }

    protected virtual Vector3 GetHackPosition()
    {
        if (hackPoint)
        {
            return hackPoint.position;
        }
        else
        {
            return transform.position;
        }
    }
    private void Awake()
    {
        if (!highlightEffect)
        {
            highlightEffect = GetComponent<HighlightEffect>();
        }


        //Initialise Hacks
        // for (var i = 0; i < hacks.Length; i++)
        // {
        //     HackAbility hackAbility = hacks[i];
        //     if (hackAbility)
        //     {
        //         hackAbility = HackManager.GetHack(hackAbility.HackName);
        //         hackAbility.Initialise(this);
        //         hacks[i] = hackAbility;
        //     }
        //     
        // }

        for (var i = 0; i < hacks.Length; i++)
        {
            if (!hacks[i])
            {
                Debug.LogError($"{name} hack is null.");
            }

            hacks[i] = Instantiate(hacks[i]);
            hacks[i].Initialise(this);
            hacks[i].name = hacks[i].name.Replace("(Clone)", "").Trim();
        }

        if (!audioDistract)
        {
            audioDistract = GetComponent<SO_AudioDistraction>();
        }

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
        onHackSelectEvent.Invoke();
    }

    public virtual void OnSelect_Exit()
    {
        highlightEffect.highlighted = false;
    }

    public void AddHack(HackAbility hackAbility)
    {
        List<HackAbility> temp = new List<HackAbility>(hacks);
        temp.Add(hackAbility);
        hacks = (temp.ToArray());
    }

    /// <summary>
    /// For activating the hack at index i
    /// Might need to override this if the hack needs to pass a special Hack Context
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public virtual int ActivateHack(int i, HackContext_Enum[] hackContextEnum = default)
    {
        if (CheckHackIndex(i))
        {
            Debug.LogError($"{name} active hack index {i} out of range {hacks.Length}.");
            return 1;
        }

        if (hacks[i].IsHackable)
        {
            hacks[i].Hack(new HackContext(new[] { this }, hackContextEnum));
        }
        else
        {
            Debug.LogError($"{name} active hack index {i} can not hack.");
            return 2;
        }

        onHackActivateEvent.Invoke();
        return 0;
    }

    // public virtual int ActivateHack<T>(HackContext_Enum[] hackContextEnum = default)
    // {
    //     for (var index = 0; index < hacks.Length; index++)
    //     {
    //         HackAbility hackAbility = hacks[index];
    //         if (typeof(T) == hackAbility.GetType())
    //         {
    //             return ActivateHack(index, hackContextEnum);
    //         }
    //     }
    //
    //     return 1;
    // }

    /// <summary>
    /// Check if index is valid
    /// true if it fails
    /// </summary>
    /// <param name="i"></param>
    /// <returns>true if it fails</returns>
    protected bool CheckHackIndex(int i)
    {
        return i >= hacks.Length || i < 0;
    }

    public int GetHackIndex<T>()
    {
        for (var index = 0; index < hacks.Length; index++)
        {
            HackAbility hackAbility = hacks[index];
            if (typeof(T) == hackAbility.GetType())
            {
                return index;
            }
        }

        return -1;
    }


    [ContextMenu("Test hack 0")]
    public void TestHack0()
    {
        ActivateHack(0);
    }

    public virtual void CreateAudioDistraction()
    {
        if (audioDistract)
        {
            audioDistract.CreateAudioDistraction(this, transform.position);
        }
        else
        {
            Debug.Log($"{name}: Does not allow audio distraction");
        }
    }


    public virtual void DisplayTutorial(TutorialEnum tutorialEnum)
    {
        TutorialManager.Display_FirstTime(tutorialEnum);
    }

    public virtual void DisplayTutorial(TutorialInstructions tutorialEnum)
    {
        //TODO: convert to just take the scriptable object data
        TutorialManager.Display_FirstTime(tutorialEnum.TutorialEnum);
    }

    public virtual void Hack_ChangeMaterial()
    {
        if (dataEffect)
        {
            dataEffect.SetVector3("TPosition",Camera.main.transform.position+new Vector3(0,-.1f,0));
            dataEffect.Play();
        }
        List<Material> tempList = new List<Material>();

        if (goldlessMaterial)
        {
            foreach (MeshRenderer r in renderers)
            {
                tempList = new List<Material>();
                for (int i = 0; i < r.materials.Length; i++)
                {
                    tempList.Add(goldlessMaterial);
                }

                r.materials = tempList.ToArray();

            }
        }
    }
}