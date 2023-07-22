using UnityEngine;

namespace Task
{
    public class TaskObject : SmartObjects
    {

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

        public override void Interact(NpcController npc)
        {
            npc.PlayAnimation(NpcAnimation.Interact);
        }

        public override void OnSelect_Enter()
        {
            
        }

        public override void OnSelect_Exit()
        {
        }

        public void FinishTask(NpcController npc,bool isInterrupt = false)
        {
            npc.PlayAnimation(NpcAnimation.Idle);

        }

        public virtual TaskEvent GetTaskEvent()
        {
            return new TaskEvent("Random Event", this, 0, transform.position, 2f);
        }
    }
}
