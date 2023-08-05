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

    public List<SmartObject> AllDetectedSmartObjects => allDetectedSmartObjects;

    public List<SmartObject> AllLoSSmartObjects => allLoSSmartObjects;


    private void FixedUpdate()
    {
        UpdateLoSObjects();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.3f, 1, .3f);
        Gizmos.DrawRay(eyePositon,-transform.right*los_Distance);
    }

    void UpdateLoSObjects()
    {
        foreach (SmartObject smartObject in allDetectedSmartObjects)
        {
            Vector3 diff = (smartObject.Position - eyePositon);

            if (!allLoSSmartObjects.Contains(smartObject))
            {
                if (Physics.RaycastAll(eyePositon, diff, los_Distance, los_LayerMask).Length == 1)
                {
                    allLoSSmartObjects.Add(smartObject);
                }
            }
            else
            {
                if (Physics.RaycastAll(eyePositon, diff, los_Distance, los_LayerMask).Length != 1)
                {
                    allLoSSmartObjects.Remove(smartObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SmartObject smartObject = GetSmartObject(other);
        if (smartObject)
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