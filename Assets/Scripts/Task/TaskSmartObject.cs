using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Task
{
    public class TaskSmartObject : SmartObject
    {
        [FormerlySerializedAs("availableTasks")]
        [Header("Tasks")]
        [SerializeField]
        TaskDescription associateTask;

        [SerializeField]
        private NpcAnimation interactAnimation = NpcAnimation.Interact;

        [SerializeField]
        private List<ItemObject> currentItems;

        private bool inUse = false;

        [Header("Components")]
        [SerializeField]
        private TaskObjectController controller;

        [Header("Animation")]
        [SerializeField]
        private bool useAnimationPoint = false;
        [SerializeField]
        private Transform animationPoint;

        public bool InUse => inUse;

        public TaskDescription AssociateTask => associateTask;



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
            //Feels like this should be in the task object visual controller but cba to move it there
            if (!animationPoint)
            {
                npc.MoveTransform(InteractPosition,InteractRotation,interactAnimation);
            }
            else
            {
                npc.MoveTransform(animationPoint.position,animationPoint.rotation,interactAnimation);
            }
            
            RemoveUsedItems(npc.GetCurrentTask());
            inUse = true;
            controller?.OnInteract(npc);
        }

        /// <summary>
        /// for depositing the item
        /// probably switch depends on the item type
        /// </summary>
        /// <param name="itemObject"></param>
        public virtual void Deposit(ItemObject itemObject)
        {
            currentItems.Add(itemObject);
            controller?.OnDeposit();
        }


        public virtual void FinishTask(NpcController npc, TaskEvent taskEvent, bool isInterrupt = false)
        {
            // npc.PlayAnimation(NpcAnimation.Idle);
            npc.MoveTransform(InteractPosition,InteractRotation,NpcAnimation.Idle);

            inUse = false;
            if (isInterrupt)
            {
                controller?.OnInterruptTask();
            }
            else
            {
                controller?.OnFinishTask();
            }
        }

        public override bool Equals(object other)
        {
            if (!associateTask)
            {
                return false;
            }
            if (other is TaskDescription td)
            {
                return associateTask.Equals(td);
            }
            return base.Equals(other);
        }

        private void RemoveUsedItems(TaskEvent taskEvent)
        {
            ItemObject itemToRemove = null;

            foreach (ItemName itemName in taskEvent.RequiredItems)
            {
                foreach (ItemObject currentItem in currentItems)
                {
                    if (currentItem.Equals(itemName))
                    {
                        itemToRemove = currentItem;
                        if (taskEvent.ItemsConsumeOnUse)
                        {
                            currentItem.Destroy();
                        }
                        else
                        {
                            StartCoroutine(DelayRemoveUsedItem(currentItem, taskEvent.Duration));
                        }

                        break;
                    }
                }

                if (itemToRemove)
                {
                    currentItems.Remove(itemToRemove);
                    itemToRemove = null;
                }
            }
        }

        // public virtual void ReplaceAvailableTask(TaskEvent taskEvent)
        // {
        //     List<TaskEvent> temp = new List<TaskEvent>(associateTask);
        //     temp.Add(taskEvent);
        //     associateTask = temp.ToArray();
        // }

        // public virtual TaskEvent GetTaskEvent(int i = 0)
        // {
        //     return associateTask[i];
        // }

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

        IEnumerator DelayRemoveUsedItem(ItemObject itemObject, float time)
        {
            yield return new WaitForSeconds(time);
            itemObject.Drop(InteractPosition);
        }
    }
}