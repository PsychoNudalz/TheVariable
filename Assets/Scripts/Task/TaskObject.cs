using System;
using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    /// <summary>
    /// Swapped from struct to class as it is 2840 bytes, bigger than the recommended 16 bytes
    /// </summary>
    public class TaskObject : SmartObject
    {

        [Header("Tasks")]
        [SerializeField]
        TaskEvent[] availableTasks = Array.Empty<TaskEvent>();

        [SerializeField]
        private List<ItemObject> currentItems;

        public TaskEvent[] AvailableTasks => availableTasks;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void AwakeBehaviour()
        {
            
        }

        protected override void StartBehaviour()
        {
        }

        protected override void UpdateBehaviour()
        {
        }

        // /// <summary>
        // /// When the task is in queue and started to be executed
        // /// </summary>
        // /// <param name="npc"></param>
        // public virtual void QueueTask(NpcController npc)
        // {
        //     
        // }

        /// <summary>
        /// When the NPC is in range to interact and start the task and the object runs it's process
        /// </summary>
        /// <param name="npc"></param>
        public override void Interact(NpcController npc)
        {
            npc.PlayAnimation(NpcAnimation.Interact);
        }

        /// <summary>
        /// for depositing the item
        /// probably switch depends on the item type
        /// </summary>
        /// <param name="itemObject"></param>
        public virtual void Deposit(ItemObject itemObject)
        {
            
        }


        public virtual void FinishTask(NpcController npc,bool isInterrupt = false)
        {
            npc.PlayAnimation(NpcAnimation.Idle);

        }

        public virtual void AddAvailableTask(TaskEvent taskEvent)
        {
            List<TaskEvent> temp = new List<TaskEvent>(availableTasks);
            temp.Add(taskEvent);
            availableTasks = temp.ToArray();
        }

        public virtual TaskEvent GetTaskEvent(int i = 0)
        {
            return availableTasks[i];
        }

        public bool HasItem(ItemName item)
        {
            foreach (ItemObject currentItem in currentItems)
            {
                if (currentItem.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public ItemName[] GetMissingItems(TaskEvent task)
        {
            List<ItemName> missingItems = new List<ItemName>();
            foreach (ItemName item in task.RequiredItems)
            {
                if (!HasItem(item))
                {
                    missingItems.Add(item);
                }
            }

            return missingItems.ToArray();
        }
    }
}
