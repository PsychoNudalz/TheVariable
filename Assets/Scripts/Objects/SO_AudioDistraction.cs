using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SO_AudioDistraction:MonoBehaviour
{
[SerializeField]
    private float audioDistract_Range = 10f;

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

    [SerializeField]
    private SoundAbstract audioDistract_SFX;

    private void Awake()
    {
        audioDistraction_LayerMask = LayerMask.GetMask("Npc","Object","Environment","Ground");
    }

    public void CreateAudioDistraction(SmartObject so,Vector3 position)
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
        RaycastHit[] castHits = Physics.SphereCastAll(position, audioDistract_Range, Vector3.up, 0,
            audioDistraction_LayerMask);
        Collider collider;
        foreach (RaycastHit hit in castHits)
        {
            collider = hit.collider;
            if (collider.TryGetComponent(out NpcController npcController))
            {
                SensorySource_Audio newSSA = new SensorySource_Audio(so, audioDistract_Strength);
                newSSA.AdjustStrength(npcController.transform.position, audioDistraction_LayerMask,
                    audioDistract_Dampen);
                npcController.AddSensorySource(newSSA);
            }
        }
    }
}