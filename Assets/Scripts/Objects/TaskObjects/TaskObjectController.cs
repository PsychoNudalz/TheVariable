using System;
using UnityEngine;
using UnityEngine.Events;

namespace Task
{
    /// <summary>
    /// Handles logic for task objects
    /// custom for each object
    /// Subclasses will be called TOC_(Object)
    /// This is being depreciated, as there isn't enough time
    /// </summary>
    public abstract class TaskObjectController: MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool autoFinishOnInterrupt = true;
        [Header("UnityEvents")]
        [SerializeField]
        private UnityEvent onInteractEvent;
        [SerializeField]
        private UnityEvent onDepositEvent;
        [SerializeField]
        private UnityEvent onFinishEvent;
        [SerializeField]
        private UnityEvent onInterruptEvent;

        [Header("Components")]
        [SerializeField]
        private TaskSmartObject taskSmartObject;

        [SerializeField]
        private TaskObjectVisualController taskObjectVisualController;


        private void Awake()
        {
            if (!taskSmartObject)
            {
                taskSmartObject = GetComponent<TaskSmartObject>();
            }

            if (!taskObjectVisualController)
            {
                taskObjectVisualController = GetComponent<TaskObjectVisualController>();
            }


            
        }

        public virtual void OnInteract(NpcController npc = null)
        {
            onInteractEvent.Invoke();
            taskObjectVisualController.OnInteract(npc);
        }

        public virtual void OnDeposit()
        {
            onDepositEvent.Invoke();
            taskObjectVisualController.OnDeposit();

        }

        public virtual void OnFinishTask()
        {
            onFinishEvent.Invoke();
            taskObjectVisualController.OnFinishTask();

        }

        public virtual void OnInterruptTask()
        {
            onInterruptEvent.Invoke();
            taskObjectVisualController.OnInterruptTask();
            if (autoFinishOnInterrupt)
            {
                OnFinishTask();
            }

        }
    }
}