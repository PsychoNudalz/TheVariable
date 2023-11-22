using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcVisionConeController : MonoBehaviour
{
    [SerializeField]
    private HashSet<SmartObject> allDetectedSmartObjects = new HashSet<SmartObject>();
    // private List<SmartObject> allDetectedSmartObjectsBuffer = new List<SmartObject>();

    [Header("Line of Sight")]
    [SerializeField]
    private HashSet<SmartObject> allLoSSmartObjects = new HashSet<SmartObject>();

    [SerializeField]
    private float los_Distance = 15f;
    [SerializeField]
    private float los_DeadZone = 1f;

    private float los_CastRange => los_Distance - los_DeadZone;
    private Vector3 los_Offset => forwardDirection * los_DeadZone;

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

    public HashSet<SmartObject> AllDetectedSmartObjects => allDetectedSmartObjects;

    public HashSet<SmartObject> AllLoSSmartObjects => allLoSSmartObjects;


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
        Gizmos.DrawRay(eyePositon+los_Offset, forwardDirection *los_CastRange);
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawRay(eyePositon, los_Offset);


    }

    void UpdateLoSObjects()
    {
        List<SmartObject> objectsToRemove = new List<SmartObject>();
        HashSet<SmartObject> newLos = new HashSet<SmartObject>();
        foreach (SmartObject smartObject in allDetectedSmartObjects)
        {
            // if (!smartObject)
            // {
            //     //TODO: actually remove the null object
            //     allDetectedSmartObjects.RemoveAt(i);
            //     i--;
            //     continue;
            // }
            if (!smartObject||Vector3.Dot((smartObject.Position - transform.position).normalized, forwardDirection) <
                0)
            {
                objectsToRemove.Add(smartObject);
                // allDetectedSmartObjects.Remove(smartObject);
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
                if (detected&&smartObject.Equals(detected))
                {
                    newLos.Add(smartObject);
                    if (isDebug)
                    {
                        Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                    }
                }
                else
                {
                    if (isDebug)
                    {
                        Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                    }
                }


                // if (!allLoSSmartObjects.Contains(smartObject))
                // {
                //     //If the object wasn't in line of sight originally
                //     if (smartObject.Equals(detected))
                //     {
                //         allLoSSmartObjects.Add(smartObject);
                //         if (isDebug)
                //         {
                //             Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                //         }
                //     }
                //     else
                //     {
                //         if (isDebug)
                //         {
                //             //Remaining out of LOS, detected is null
                //             Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                //         }
                //     }
                // }
                // else
                // {
                //     //If the object is in line of sight originally
                //
                //     if (smartObject.Equals(detected))
                //     {
                //         if (isDebug)
                //         {
                //             Debug.DrawLine(eyePositon, smartObject.Position, Color.green);
                //         }
                //     }
                //     else
                //     {
                //         if (!smartObject.Equals(selfSmartObject))
                //         {
                //             allLoSSmartObjects.Remove(smartObject);
                //         }
                //
                //         if (isDebug)
                //         {
                //             Debug.DrawLine(eyePositon, smartObject.Position, Color.red);
                //         }
                //     }
                // }
            }
            allLoSSmartObjects = newLos;

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

            // // //For some reason contain is not stopping double adding the items
            // foreach (SmartObject detectedSmartObject in allDetectedSmartObjects)
            // {
            //     if (smartObject.Equals(detectedSmartObject))
            //     {
            //         // Debug.LogError($"{selfSmartObject.name}: found same Smart Object");
            //         return;
            //     }
            //
            //     // if (smartObject.name.Equals(detectedSmartObject.name))
            //     // {
            //     //     Debug.LogError($"{selfSmartObject.name}: found same Smart Object");
            //     // }
            // }
            if (!allDetectedSmartObjects.Contains(smartObject))
            {
                allDetectedSmartObjects.Add(smartObject);
            }
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

            if (allDetectedSmartObjects.Contains(smartObject))
            {
                allDetectedSmartObjects.Remove(smartObject);
            }
            
            
            // for (var i = 0; i < allDetectedSmartObjects.Count; i++)
            // {
            //     var detectedSmartObject = allDetectedSmartObjects[i];
            //     if (smartObject.Equals(detectedSmartObject))
            //     {
            //         allDetectedSmartObjects.RemoveAt(i);
            //         if (allLoSSmartObjects.Contains(smartObject))
            //         {
            //             allLoSSmartObjects.Remove(smartObject);
            //         }
            //
            //         i--;
            //     }
            // }
        }
    }

    private static SmartObject GetSmartObject(Collider other)
    {
        return other.GetComponentInParent<SmartObject>();
    }

    public void CleanUpAllDetectedSO()
    {
        List<SmartObject> temp = new List<SmartObject>();

        
        foreach (SmartObject smartObject in allDetectedSmartObjects)
        {
            
            if (!smartObject||Vector3.Dot((smartObject.Position - transform.position).normalized, forwardDirection) <
                0)
            {
                continue;
                
            }
            else if (!temp.Contains(smartObject))
            {
                temp.Add(smartObject);
            }
        }

        allDetectedSmartObjects = new HashSet<SmartObject>();
        foreach (SmartObject smartObject in temp)
        {
            allDetectedSmartObjects.Add(smartObject);
        }


        if (isDebug)
        {
            HashSet<SmartObject> wasFound = new HashSet<SmartObject>();
            foreach (SmartObject smartObject in allDetectedSmartObjects)
            {
                foreach (SmartObject o in wasFound)
                {
                    if (o.name.Equals(smartObject.name))
                    {
                        // Debug.LogError($"{name} Found same Item: {smartObject.name}");
                        if (o.GetInstanceID().Equals(smartObject.GetInstanceID()))
                        {
                            Debug.LogError($"{name} Found same ID: {smartObject.GetInstanceID()}");
                            
                        }
                    }
                }
                wasFound.Add(smartObject);
            }
        }
        // allDetectedSmartObjects = temp;
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