using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Task
{
    /// <summary>
    /// Identify where the task is, when does it start, how long it is and what TaskObject it is interacting with
    /// </summary>
    [Serializable]
    [CanBeNull]
    public class TaskEvent
    {
        [SerializeField]
        private string taskName;

        [SerializeField]
        private TaskObject taskObject;

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

        public TaskObject TaskObject => taskObject;

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

        public bool HasObject => taskObject != null;


        public TaskEvent()
        {
            this.taskName = "";
            this.position = default;
            this.duration = 1;
            this.startTime = -1;
            taskObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName)
        {
            this.taskName = taskName;
            this.position = default;
            this.duration = 1;
            this.startTime = -1;
            taskObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.position = position;
            this.duration = duration;
            this.startTime = startTime;
            taskObject = null;
            requiredItems = Array.Empty<ItemName>();
            itemsConsumeOnUse = false;
        }

        public TaskEvent(string taskName, TaskObject taskObject, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.taskObject = taskObject;
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

            if (!taskObject)
            {
                return flag;
            }

            foreach (ItemName requiredItem in requiredItems)
            {
                if (!taskObject.HasItem(requiredItem))
                {
                    Debug.Log($"{taskName} is missing {requiredItem.ToString()}");
                    temp.Add(requiredItem);
                    flag = false;
                }
            }
            items = temp.ToArray();
            return flag;
        }
    }
}