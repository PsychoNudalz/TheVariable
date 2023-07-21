using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct TaskEvent
    {
        [SerializeField]
        private string taskName;

        [SerializeField]
        private int startTime;

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        float duration;

        public string TaskName => taskName;

        public int StartTime => startTime;

        public Vector3 Position => position;

        public float Duration => duration;

        public TaskEvent(string taskName, int startTime, Vector3 position, float duration)
        {
            this.taskName = taskName;
            this.position = position;
            this.duration = duration;
            this.startTime = startTime;
        }
    }
}