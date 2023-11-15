using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    [System.Serializable]
    public abstract class Node
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [SerializeField]
        public State state = State.Running;

        public bool IsRunning => state == State.Running;

        [HideInInspector]
        public bool started = false;

        [HideInInspector]
        public string guid = System.Guid.NewGuid().ToString();

        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public Context context;


        [HideInInspector] public NpcController npcController => context.NpcController;

        [HideInInspector]
        public Blackboard blackboard;

        [HideInInspector] public Vector3 agent_Position => context.transform.position;


        [TextArea]
        public string name;

        [TextArea]
        public string description;

        public bool drawGizmos = false;

        public string NodeTypeName => GetType().Name;

        protected Node()
        {
            name = GetType().Name;
        }

        public string GetName()
        {
            // if (name.Length == 0)
            // {
            //     name = GetType().Name;
            // }

            return name;
        }

        public State Update()
        {
            if (!started)
            {
                started = true;
                OnStart();
            }

            state = OnUpdate();

            if (state != State.Running)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        public virtual void Abort()
        {
            BehaviourTree.Traverse(this, (node) =>
            {
                node.OnStop();
                node.started = false;
                node.state = State.Running;


                // if (node is ActionNode an)
                // {
                //     if (an.started || an.IsRunning)
                //     {
                //         an.OnStop();
                //     }
                // }
                // else
                // {
                //     node.OnStop();
                // }
                // node.started = false;
                // node.state = State.Running;
            });
        }

        public virtual void OnDrawGizmos()
        {
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        //Shared Methods
        protected State IsAlive()
        {
            blackboard.health = context.NpcController.Health;
            if (blackboard.health > blackboard.healthThreshold)
            {
                return State.Success;
            }
            else
            {
                return State.Failure;
            }
        }

        protected bool AllowedState(NPC_AlertState[] states)
        {
            foreach (NPC_AlertState npcAlertState in states)
            {
                if (npcAlertState.Equals(blackboard.alertState))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Change blackboard Alert State and update alert value
        /// Handles events for when the state changes
        /// </summary>
        /// <param name="alertState"></param>
        /// <param name="overrideValue"> if the alert value be override to use the minimum alert value, or the maximum between it and current  </param>
        public void ChangeAlertState(NPC_AlertState alertState, bool overrideValue)
        {
            // Debug.Log($"Node change NPC state: {blackboard.alertState} --> {alertState}");
            blackboard.alertState = alertState;
            if (alertState == NPC_AlertState.Suspicious)
            {
                GameManager.Alert_Suspicious();
            }

            if (alertState == NPC_AlertState.Spotted)
            {
                context.NpcController.SpotPlayer();
            }

            if (overrideValue)
            {
                context.NpcController.Override_AlertValue(alertState);
            }
            else
            {
                context.NpcController.Set_MinAlertValue(alertState);
            }

            context.NpcController.UpdateAlertUI();
        }

        protected void Clear_Blackboard_Items()
        {
            blackboard.missingItem = ItemName.None;
            blackboard.locatedItem = null;
            blackboard.pickedUpItem = null;
        }

        protected void Update_LastKnown(CameraController camera)
        {
            if (!camera)
            {
                return;
            }

            CameraController blackboardPlayerLastKnownCamera = camera;
            blackboard.player_LastKnown_Camera = blackboardPlayerLastKnownCamera;
            if (blackboard.player_LastKnown_Camera)
            {
                blackboard.player_LastKnown_Position = blackboard.player_LastKnown_Camera.Position;
                blackboard.player_LastKnown_Time = Time.time;
            }
        }

        protected Quaternion GetDirectionToNpc(NpcObject npcObject)
        {
            Vector3 dir = (npcObject.Position - agent_Position).normalized;
            return Quaternion.AngleAxis(Vector2.Angle(new Vector2(0, 1f), new Vector2(dir.x, dir.z)),
                Vector3.up);
        }
    }


    public class NullNodeException : Exception
    {
        private Node node;

        public NullNodeException(Node node)
        {
            this.node = node;
        }

        public override string ToString()
        {
            string n = node.GetName();

            return $"{n}: missing child node. \nDescription: {node.description}";
        }
    }
}