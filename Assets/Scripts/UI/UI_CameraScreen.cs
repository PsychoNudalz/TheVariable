using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_CameraScreen : MonoBehaviour
{
    [SerializeField]
    private Animator animator;


    public void PlayAnimation(CameraInvestigationMode cameraInvestigationMode)
    {
        switch (cameraInvestigationMode)
        {
            case CameraInvestigationMode.None:
                animator.SetBool("Investigate", false);
                animator.SetBool("Spotted", false);
                break;
            case CameraInvestigationMode.Investigated:
                animator.SetBool("Investigate", true);
                animator.SetBool("Spotted", false);
                break;
            case CameraInvestigationMode.Spotted:
                animator.SetBool("Investigate", false);
                animator.SetBool("Spotted", true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cameraInvestigationMode), cameraInvestigationMode, null);
        }
    }
}