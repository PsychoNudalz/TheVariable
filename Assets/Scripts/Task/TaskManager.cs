using UnityEngine;

namespace Task
{
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
    }
}