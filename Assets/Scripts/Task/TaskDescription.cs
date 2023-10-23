using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// /// <summary>
// /// Enum to save what task it is, used for NPC to querying task object
// /// </summary>
// public enum TaskEnum
// {
//     NONE,
//     BAKE_CAKE
// }

/// <summary>
///Saves the details of the Task
/// 
/// </summary>
[CreateAssetMenu(menuName = "Task/TaskDescription")]
[Serializable]
public class TaskDescription : ScriptableObject
{
    [SerializeField]
    private string taskName;

    // [SerializeField]
    // private TaskEnum taskEnum;

    [SerializeField]
    float duration;

    [SerializeField]
    private ItemName[] requiredItems;

    [SerializeField]
    private bool itemsConsumeOnUse;

    public string TaskName => taskName;

    public float Duration => duration;

    public ItemName[] RequiredItems => requiredItems;

    public bool ItemsConsumeOnUse => itemsConsumeOnUse;

    public TaskDescription()
    {
        taskName = "";
        // taskEnum = TaskEnum.NONE;
        duration = 0;
        requiredItems = Array.Empty<ItemName>();
        itemsConsumeOnUse = false;
    }

    // TaskEnum autoFindTaskEnum(string s)
    // {
    //     string taskEnumString = "";
    //     int matchScore = 0;
    //     int previousMatchScore = 0;
    //     TaskEnum currentSelectedTaskEnum = TaskEnum.NONE;
    //     foreach (TaskEnum currentTaskEnum in (TaskEnum[]) Enum.GetValues(typeof(TaskEnum)))
    //     {
    //         matchScore = 0;
    //         foreach (var VARIABLE in COLLECTION)
    //         {
    //             
    //         }
    //     }
    // }
}