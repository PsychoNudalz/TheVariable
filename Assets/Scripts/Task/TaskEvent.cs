﻿using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Task
{
    /// <summary>
    /// Identify where the task is, when does it start, how long it is and what TaskObject it is interacting with
    /// </summary>
    [Serializable]
    public struct TaskEvent
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

        public string TaskName => taskName;

        public TaskObject TaskObject => taskObject;

        public int StartTime => startTime;

        public bool IsNull => taskName.Equals("");

        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public float Duration => duration;

        public bool HasObject => taskObject != null;



        public TaskEvent(string taskName)
        {
            this.taskName = taskName;
            this.position = default;
            this.duration = 1;
            this.startTime = -1;
            taskObject = null;
        }

        public TaskEvent(string taskName, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.position = position;
            this.duration = duration;
            this.startTime = startTime;
            taskObject = null;
        }

        public TaskEvent(string taskName, TaskObject taskObject, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.taskObject = taskObject;
            this.startTime = startTime;
            this.position = position;
            this.duration = duration;
        }
    }
}