using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Task
{


    
    
    /// <summary>
    /// Identify where the task is, when does it start, how long it is and what TaskObject it is interacting with
    /// Swapped from struct to class as it is 2840 bytes, bigger than the recommended 16 bytes
    /// </summary>
    [Serializable]
    [CanBeNull]
    public class TaskEvent
    {

        [SerializeField]
        private TaskDescription taskDescription;

        [SerializeField]
        private int startTime;

        // [SerializeField]
        private TaskSmartObject taskSmartObject;


        public TaskDescription TaskDescription => taskDescription;

        public int StartTime => startTime;

        // public bool IsNull => taskName.Equals("");

        public ItemName[] RequiredItems => taskDescription.RequiredItems;

        public bool ItemsConsumeOnUse => taskDescription.ItemsConsumeOnUse;


        public float Duration => taskDescription.Duration;
        public string TaskName => taskDescription.TaskName;
        public TaskSmartObject TaskSmartObject => taskSmartObject;
        public bool HasObjectSet => taskSmartObject != null;
        public Vector3 Position
        {
            get => taskSmartObject.Position;
            // set => ;
        }


        // public bool HasObject => taskSmartObject != null;


        public TaskEvent()
        {
            // this.taskName = "";
            // this.position = default;
            // // this.duration = 1;
            // this.startTime = -1;
            // taskSmartObject = null;
            // requiredItems = Array.Empty<ItemName>();
            // itemsConsumeOnUse = false;
        }
        //
        // public TaskEvent(string taskName)
        // {
        //     this.taskName = taskName;
        //     this.position = default;
        //     this.duration = 1;
        //     this.startTime = -1;
        //     taskSmartObject = null;
        //     requiredItems = Array.Empty<ItemName>();
        //     itemsConsumeOnUse = false;
        // }
        //
        // public TaskEvent(string taskName, int startTime, Vector3 position, float duration)
        // {
        //     this.taskName = taskName;
        //     this.position = position;
        //     this.duration = duration;
        //     this.startTime = startTime;
        //     taskSmartObject = null;
        //     requiredItems = Array.Empty<ItemName>();
        //     itemsConsumeOnUse = false;
        // }
        //
        // public TaskEvent(string taskName, TaskSmartObject taskSmartObject, int startTime, Vector3 position, float duration)
        // {
        //     this.taskName = taskName;
        //     this.taskSmartObject = taskSmartObject;
        //     this.startTime = startTime;
        //     this.position = position;
        //     this.duration = duration;
        //     requiredItems = Array.Empty<ItemName>();
        //     itemsConsumeOnUse = false;
        // }

        public bool CanStartTask(out ItemName[] items)
        {
            List<ItemName> temp = new List<ItemName>();
            items = Array.Empty<ItemName>();
            bool flag = true;
            //
            // if (!taskSmartObject)
            // {
            //     return flag;
            // }

            foreach (ItemName requiredItem in taskDescription.RequiredItems)
            {
                if (!taskSmartObject.HasItem(requiredItem))
                {
                    // Debug.Log($"{taskName} is missing {requiredItem.ToString()}");
                    temp.Add(requiredItem);
                    flag = false;
                }
            }
            items = temp.ToArray();
            return flag;
        }

        public void SetTaskObject(TaskSmartObject taskSmartObject)
        {
            this.taskSmartObject = taskSmartObject;
        }
    }
}