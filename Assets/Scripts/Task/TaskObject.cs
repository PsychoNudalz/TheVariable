using UnityEngine;

namespace Task
{
    public class TaskObject : MonoBehaviour
    {
        [SerializeField]
        private Transform interactPoint;

        public Vector3 Position => InteractPointPosition();

        private Vector3 InteractPointPosition()
        {
            if (!interactPoint)
            {
                return transform.position;
            }
            return interactPoint.position;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public virtual void Interact()
        {
            
        }
        
        public virtual void Finish(bool isInterrupt = false)
        {
            
        }

        public virtual TaskEvent GetTaskEvent()
        {
            return new TaskEvent("Random Event", this, 0, transform.position, 2f);
        }
    }
}
