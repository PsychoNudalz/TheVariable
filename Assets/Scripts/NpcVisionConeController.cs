using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVisionConeController : MonoBehaviour
{
    [SerializeField]
    private List<SmartObject> allDetectedSmartObjects;

    [Header("Line of Sight")]
    [SerializeField]
    private List<SmartObject> allLoSSmartObjects;

    [SerializeField]
    private float los_Distance = 15f;

    [SerializeField]
    private LayerMask los_LayerMask;

    [SerializeField]
    private Transform eyeTransform;

    private Vector3 eyePositon => eyeTransform.position;

    [Header("Component")]
    [SerializeField]
    private SmartObject objectIgnore;

    [SerializeField]
    private SmartObject selfSmartObject;

    public List<SmartObject> AllDetectedSmartObjects => allDetectedSmartObjects;

    public List<SmartObject> AllLoSSmartObjects => allLoSSmartObjects;
    
    

     static string CameraTag = "CCTV";

     private void Awake()
     {
         if (selfSmartObject)
         {
             allDetectedSmartObjects.Add(selfSmartObject);
             allLoSSmartObjects.Add(selfSmartObject);
         }
     }

     private void FixedUpdate()
    {
        UpdateLoSObjects();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.3f, 1, .3f);
        Gizmos.DrawRay(eyePositon, -transform.right * los_Distance);
    }

    void UpdateLoSObjects()
    {
        foreach (SmartObject smartObject in allDetectedSmartObjects)
        {
            Vector3 diff = (smartObject.ColliderPosition - eyePositon);

            // int detectedObjects = Physics.RaycastAll(eyePositon, diff, los_Distance, los_LayerMask).Length;
            RaycastHit hit;
            bool detectedObjects = Physics.Raycast(eyePositon, diff,out hit, los_Distance, los_LayerMask);

            if (detectedObjects)
            {
                SmartObject detected = hit.collider.GetComponentInParent<SmartObject>();
                if (!allLoSSmartObjects.Contains(smartObject))
                {
                    //If the object wasn't in line of sight originally
                    if (smartObject.Equals(detected))
                    {
                        allLoSSmartObjects.Add(smartObject);
                        Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                    }
                    else
                    {
                        //Remaining out of LOS, detected is null
                        Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                    }
                }
                else
                {
                    //If the object is in line of sight originally

                    if (smartObject.Equals(detected))
                    {
                        Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                    }
                    else
                    {
                        if (!smartObject.Equals(selfSmartObject))
                        {
                            allLoSSmartObjects.Remove(smartObject);
                        }

                        Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SmartObject smartObject = GetSmartObject(other);
        if (smartObject && !allDetectedSmartObjects.Contains(smartObject))
        {
            if (smartObject.Equals(objectIgnore))
            {
                return;
            }

            allDetectedSmartObjects.Add(smartObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        SmartObject smartObject = GetSmartObject(other);
        if (selfSmartObject.Equals(smartObject))
        {
            return;
        }
        if (smartObject)
        {
            if (allDetectedSmartObjects.Contains(smartObject))
            {
                allDetectedSmartObjects.Remove(smartObject);
                allLoSSmartObjects.Remove(smartObject);
            }
        }
    }

    private static SmartObject GetSmartObject(Collider other)
    {
        return other.GetComponentInParent<SmartObject>();
    }
}