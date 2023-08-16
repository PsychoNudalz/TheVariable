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
        private string taskName;

        [FormerlySerializedAs("taskObject")]
        [SerializeField]
        private TaskSmartObject taskSmartObject;

        [SerializeField]
        private int startTime;

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        float duration;

        [SerializeField]
        private ItemName[] requiredItems;

        [SerializeField]
        private bool itemsConsumeOnUse;

        public string TaskName => taskName;

        public TaskSmartObject TaskSmartObject => taskSmartObject;

        public int StartTime => startTime;

        public bool IsNull => taskName.Equals("");

        public ItemName[] RequiredItems => requiredItems;

        public bool ItemsConsumeOnUse => itemsConsumeOnUse;

        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public float Duration => duration;

        public bool HasObject => taskSmartObject != null;


        public TaskEvent()
        {
            this.taskName = "";
            this.position = default;
            this.duration = 1;
            this.startTime = -1;
            taskSmartObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName)
        {
            this.taskName = taskName;
            this.position = default;
            this.duration = 1;
            this.startTime = -1;
            taskSmartObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.position = position;
            this.duration = duration;
            this.startTime = startTime;
            taskSmartObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName, TaskSmartObject taskSmartObject, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.taskSmartObject = taskSmartObject;
            this.startTime = startTime;
            this.position = position;
            this.duration = duration;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public bool CanStartTask(out ItemName[] items)
        {
            List<ItemName> temp = new List<ItemName>();
            items = Array.Empty<ItemName>();
            bool flag = true;

            if (!taskSmartObject)
            {
                return flag;
            }

            foreach (ItemName requiredItem in requiredItems)
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
    }
}