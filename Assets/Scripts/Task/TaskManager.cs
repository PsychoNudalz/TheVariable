using UnityEngine;

namespace Task
{
    /// <summary>
    /// Keeps tracks of world times
    /// Handles tasks
    /// </summary>
    public class TaskManager : MonoBehaviour
    {
        [SerializeField]
        private int currentTick;

        [Space(15)]
        [SerializeField]
        private int TickLoop = 48;

        [SerializeField]
        private float TimePerTick = 30;
        
        
        [Space(15)]


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
    }
}