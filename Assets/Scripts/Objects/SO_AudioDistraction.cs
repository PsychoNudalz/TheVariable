using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SO_AudioDistraction : MonoBehaviour
{
    [SerializeField]
    private float audioDistract_Range = 10f;

    [SerializeField]
    private int audioDistract_MaxNPC = 2;

    [SerializeField]
    private float audioDistract_Strength = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float audioDistract_Dampen = 1f;

    // [SerializeField]
    LayerMask audioDistraction_LayerMask;

    [SerializeField]
    private ParticleSystem audioDistract_PS;

    [SerializeField]
    private VisualEffect audioDistract_VE;

    [Space(8)]
    [SerializeField]
    private bool useGlobal = true;

    [SerializeField]
    private SoundAbstract audioDistract_SFX;

    private SmartObject savedSO;
    private Vector3 savedPosition;

    private void Awake()
    {
        audioDistraction_LayerMask = LayerMask.GetMask("Npc", "Object", "Environment", "Ground");
        if (audioDistract_VE)
        {
            audioDistract_VE.gameObject.layer = LayerMask.NameToLayer("UI_World");
        }
    }

    public void CreateAudioDistraction(SmartObject so, Vector3 position)
    {
        if (audioDistract_PS)
        {
            audioDistract_PS.Play();
        }

        if (audioDistract_VE)
        {
            audioDistract_VE.Play();
        }

        if (audioDistract_SFX)
        {
            audioDistract_SFX.Play();
        }

        if (useGlobal)
        {
            SoundManager.PlayGlobal(SoundGlobal.Distraction);
        }

        savedSO = so;
        savedPosition = position;
        Invoke(nameof(CreateSensorySources), .5f);
    }

    private void CreateSensorySources()
    {
        RaycastHit[] castHits = Physics.SphereCastAll(savedPosition, audioDistract_Range, Vector3.up, 0,
            audioDistraction_LayerMask);
        Collider collider;
        int numberOfNPCs = 0;
        foreach (RaycastHit hit in castHits)
        {
            collider = hit.collider;
            if (collider.TryGetComponent(out NpcController npcController))
            {
                if (numberOfNPCs < audioDistract_MaxNPC)
                {
                    SensorySource_Audio newSSA = new SensorySource_Audio(savedSO, audioDistract_Strength);
                    newSSA.AdjustStrength(npcController.transform.position, audioDistraction_LayerMask,
                        audioDistract_Dampen);
                    npcController.AddSensorySource(newSSA);
                }
                else
                {
                    SensorySource_Audio newSSA =
                        new SensorySource_Audio(savedSO,collider.transform.position, audioDistract_Strength*.5f);
                    newSSA.AdjustStrength(npcController.transform.position, audioDistraction_LayerMask,
                        audioDistract_Dampen);
                    npcController.AddSensorySource(newSSA);
                }

                numberOfNPCs++;
            }
        }
    }
}