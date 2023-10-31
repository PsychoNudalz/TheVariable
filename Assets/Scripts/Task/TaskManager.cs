using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Task
{
    public enum TaskQueryType
    {
        Closest,
        Random
    }

    /// <summary>
    /// Keeps tracks of world ticks
    /// Handles tasks and task Objects, allows NPC to query task object locations
    /// </summary>
    public class TaskManager : MonoBehaviour
    {
        [SerializeField]
        private int currentTick;

        [Space(10)]
        [Header("Tick Settings")]
        [SerializeField]
        private int TickLoop = 48;

        [SerializeField]
        private float TimePerTick = 30;


        [Space(10)]
        [Header("Task Settings")]
        [SerializeField]
        private TaskSmartObject[] allTaskSmartObjects;


        private float timePerTickCurrent;

        public static TaskManager current;

        public static int Tick => current.currentTick % current.TickLoop;

        private void Awake()
        {
            current = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            BroadcastTick();
            if (allTaskSmartObjects.Length == 0)
            {
                InitialiseAllTaskSOs();
            }
        }

        [ContextMenu("Initialise All TaskSOs")]
        public void InitialiseAllTaskSOs()
        {
            allTaskSmartObjects = FindObjectsOfType<TaskSmartObject>();
        }

        // Update is called once per frame
        void Update()
        {
            timePerTickCurrent += Time.deltaTime;
            if (timePerTickCurrent > TimePerTick)
            {
                currentTick++;
                BroadcastTick();
                timePerTickCurrent -= TimePerTick;
            }
        }

        public int BroadcastTick()
        {
            int i = 0;
            foreach (NpcController NPC in NpcManager.NPCs)
            {
                if (NPC.UpdateTaskFromSchedule(currentTick))
                {
                    i++;
                }
            }

            return i;
        }

        public float TickToRealTime(float tickTime)
        {
            return tickTime * TimePerTick;
        }

        public bool HasObject(TaskDescription taskDescription)
        {
            foreach (TaskSmartObject taskSmartObject in allTaskSmartObjects)
            {
                if (taskSmartObject.Equals(taskDescription))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the closest task object with the given task
        /// </summary>
        /// <param name="originPosition"></param>
        /// <param name="taskDescription"></param>
        /// <returns></returns>
        public static TaskSmartObject QueryTask(Vector3 originPosition, TaskDescription taskDescription,
            TaskQueryType taskQueryType)
        {
            TaskSmartObject[] currentAllTaskSmartObjects = current.allTaskSmartObjects;
            if (currentAllTaskSmartObjects.Length == 0)
            {
                Debug.LogError("No task object in Task Manager");
                return null;
            }


            List<TaskSmartObject> matchingSmartObjects = new List<TaskSmartObject>();
            foreach (TaskSmartObject taskSmartObject in currentAllTaskSmartObjects)
            {
                if (taskSmartObject.Equals(taskDescription) && !taskSmartObject.InUse)
                {
                    matchingSmartObjects.Add(taskSmartObject);
                }
            }

            if (matchingSmartObjects.Count == 0)
            {
                return null;
            }

            TaskSmartObject mostCompatibleTSO = null;

            switch (taskQueryType)
            {
                case TaskQueryType.Closest:
                    float currentDistance = Mathf.Infinity;
                    NavMeshPath path = new NavMeshPath();
                    float pathLength = 0;

                    LayerMask layerMask = LayerMask.GetMask("Ground");
                    Vector3 castPosition = originPosition;

                    if (Physics.Raycast(originPosition, Vector3.down, out RaycastHit hit, 5, layerMask))
                    {
                        castPosition = hit.point;
                    }

                    foreach (TaskSmartObject taskSmartObject in matchingSmartObjects)
                    {
                        if (!NavMesh.CalculatePath(castPosition, taskSmartObject.Position, NavMesh.AllAreas, path))
                        {
                            NavMesh.CalculatePath(castPosition, taskSmartObject.InteractPosition, NavMesh.AllAreas,
                                path);
                        }

                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            // Debug.Log("PathComplete");
                            pathLength = 0;
                            for (int i = 1; i < path.corners.Length; i++)
                            {
                                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                            }

                            if (pathLength < currentDistance)
                            {
                                currentDistance = pathLength;
                                mostCompatibleTSO = taskSmartObject;
                            }
                        }
                    }

                    break;
                case TaskQueryType.Random:
                    mostCompatibleTSO = matchingSmartObjects[Random.Range(0, matchingSmartObjects.Count - 1)];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(taskQueryType), taskQueryType, null);
            }


            return mostCompatibleTSO;
        }
    }
}