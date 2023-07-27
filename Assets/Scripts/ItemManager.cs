using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Singleton manager that handles items and item finding
/// </summary>
public class ItemManager : MonoBehaviour
{
    private List<ItemObject> items = new List<ItemObject>();
    public static ItemManager current;

    private void Awake()
    {
        current = this;
        items = new List<ItemObject>(FindObjectsByType<ItemObject>(FindObjectsInactive.Include,
            FindObjectsSortMode.InstanceID));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public ItemObject FindItem(ItemName itemName, Vector3 fromPosition)
    {
        bool cleanFlag = false;
        ItemObject foundItem = null;
        float foundItemDistance = -1;
        float currentItemDistance = -1;
        foreach (ItemObject item in items)
        {
            if (item)
            {
                if (item.Equals(itemName) && item.IsFree)
                {
                    if (!foundItem)
                    {
                        foundItemDistance = GetDistance(fromPosition, item.Position);
                        if (foundItemDistance > 0)
                        {
                            foundItem = item;
                        }
                    }
                    else
                    {
                        currentItemDistance = GetDistance(fromPosition, item.Position);
                        if (foundItemDistance > currentItemDistance)
                        {
                            foundItemDistance = currentItemDistance;
                            foundItem = item;
                        }
                    }
                }
            }
            else
            {
                cleanFlag = true;
            }
        }

        if (cleanFlag)
        {
            CleanList();
        }

        return foundItem;
    }

    float GetDistance(Vector3 item1, Vector3 item2)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = 0;
        if (NavMesh.CalculatePath(item1, item2, NavMesh.AllAreas, path))
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return distance;
        }

        return -1;
    }

    public void CleanList()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i]==null)
            {
                items.RemoveAt(i);
                i--;
            }
        }
    }
}