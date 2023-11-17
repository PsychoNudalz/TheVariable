using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVisionConeController : MonoBehaviour
{
    [SerializeField]
    private List<SmartObject> allDetectedSmartObjects;
    private List<SmartObject> allDetectedSmartObjectsBuffer = new List<SmartObject>();

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

    [Header("Debug")]
    [SerializeField]
    private bool isDebug = false;

    private Vector3 forwardDirection=>-transform.right;

    public List<SmartObject> AllDetectedSmartObjects => allDetectedSmartObjects;

    public List<SmartObject> AllLoSSmartObjects => allLoSSmartObjects;


    // static string CameraTag = "CCTV";

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
        // CleanUpAllDetectedSO();
        // EvaluateBuffer();
        UpdateLoSObjects();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(.3f, 1, .3f);
        Gizmos.DrawRay(eyePositon, forwardDirection * los_Distance);
    }

    void UpdateLoSObjects()
    {
        for (var i = 0; i < allDetectedSmartObjects.Count; i++)
        {
            var smartObject = allDetectedSmartObjects[i];
            // if (!smartObject)
            // {
            //     //TODO: actually remove the null object
            //     allDetectedSmartObjects.RemoveAt(i);
            //     i--;
            //     continue;
            // }
            if (!allDetectedSmartObjects[i]||Vector3.Dot((allDetectedSmartObjects[i].Position - transform.position).normalized, forwardDirection) <
                0)
            {
                allDetectedSmartObjects.RemoveAt(i);
                i--;
                continue;
            }

            Vector3 diff = Vector3.zero;
            if (smartObject is NpcObject npcObject)
            {
                diff = (npcObject.ColliderPosition + new Vector3(0, .5f, 0) - eyePositon);
            }
            else
            {
                diff = (smartObject.ColliderPosition - eyePositon);
            }

            diff = diff.normalized;
            // int detectedObjects = Physics.RaycastAll(eyePositon, diff, los_Distance, los_LayerMask).Length;
            RaycastHit hit;
            bool detectedObjects = Physics.Raycast(eyePositon, diff, out hit, los_Distance, los_LayerMask);
            // Debug.DrawRay(eyePositon, diff * los_Distance, Color.magenta);

            if (detectedObjects)
            {
                // Debug.DrawLine(eyePositon, hit.collider.transform.position, Color.cyan);

                SmartObject detected = hit.collider.GetComponentInParent<SmartObject>();
                if (!allLoSSmartObjects.Contains(smartObject))
                {
                    //If the object wasn't in line of sight originally
                    if (smartObject.Equals(detected))
                    {
                        allLoSSmartObjects.Add(smartObject);
                        if (isDebug)
                        {
                            Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                        }
                    }
                    else
                    {
                        if (isDebug)
                        {
                            //Remaining out of LOS, detected is null
                            Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                        }
                    }
                }
                else
                {
                    //If the object is in line of sight originally

                    if (smartObject.Equals(detected))
                    {
                        if (isDebug)
                        {
                            Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                        }
                    }
                    else
                    {
                        if (!smartObject.Equals(selfSmartObject))
                        {
                            allLoSSmartObjects.Remove(smartObject);
                        }

                        if (isDebug)
                        {
                            Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SmartObject smartObject = GetSmartObject(other);
        if (smartObject )
        {
            if (smartObject.Equals(objectIgnore))
            {
                return;
            }

            // //For some reason contain is not stopping double adding the items
            foreach (SmartObject detectedSmartObject in allDetectedSmartObjects)
            {
                if (smartObject.Equals(detectedSmartObject))
                {
                    // Debug.LogError($"{selfSmartObject.name}: found same Smart Object");
                    return;
                }
            
                // if (smartObject.name.Equals(detectedSmartObject.name))
                // {
                //     Debug.LogError($"{selfSmartObject.name}: found same Smart Object");
                // }
            }
            allDetectedSmartObjects.Add(smartObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        SmartObject smartObject = GetSmartObject(other);
        if (!smartObject || selfSmartObject.Equals(smartObject))
        {
            return;
        }

        if (smartObject)
        {
            for (var i = 0; i < allDetectedSmartObjects.Count; i++)
            {
                var detectedSmartObject = allDetectedSmartObjects[i];
                if (smartObject.Equals(detectedSmartObject))
                {
                    allDetectedSmartObjects.RemoveAt(i);
                    if (allLoSSmartObjects.Contains(smartObject))
                    {
                        allLoSSmartObjects.Remove(smartObject);
                    }

                    i--;
                }
            }
        }
    }

    private static SmartObject GetSmartObject(Collider other)
    {
        return other.GetComponentInParent<SmartObject>();
    }

    public void CleanUpAllDetectedSO()
    {
        List<SmartObject> temp = new List<SmartObject>();
        for (int i = 0; i < allDetectedSmartObjects.Count; i++)
        {
            if (!allDetectedSmartObjects[i]||Vector3.Dot((allDetectedSmartObjects[i].Position - transform.position).normalized, forwardDirection) <
                0)
            {
                allDetectedSmartObjects.RemoveAt(i);
                i--;
            }
            else if (!temp.Contains(allDetectedSmartObjects[i]))
            {
                temp.Add(allDetectedSmartObjects[i]);
            }
        }

        allDetectedSmartObjects = temp;
    }

    // void EvaluateBuffer()
    // {
    //     foreach (SmartObject smartObject in allDetectedSmartObjectsBuffer)
    //     {
    //         if (!allDetectedSmartObjects.Contains(smartObject))
    //         {
    //             allDetectedSmartObjects.Add(smartObject);
    //         }
    //     }
    //
    //     allDetectedSmartObjectsBuffer = new List<SmartObject>();
    // }
    
    
}