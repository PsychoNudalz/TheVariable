using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheKiwiCoder
{
    [System.Serializable]
    public class Action_SetWait : ActionNode
    {
        public float duration;
        public bool playWaitAnimation = false;

        protected float startTime
        {
            get => blackboard.wait_startTime;
            set => blackboard.wait_startTime = value;
        }


        public float bb_duration
        {
            get => blackboard.wait_duration;
            set => blackboard.wait_duration = value;
        }

        protected override void OnStart()
        {
            Wait_Start();
        }

        protected void Wait_Start()
        {
            blackboard.flag_wait = true;
            startTime = Time.time;
            bb_duration = duration;
            if (playWaitAnimation)
            {
                context.NpcController.PlayAnimation(NpcAnimation.Idle);
            }
        }

        protected void Wait_End()
        {
            blackboard.flag_wait = false;
        }

        protected override void OnStop()
        {
            Wait_End();
        }

        protected override State OnUpdate()
        {
            return Wait_Update();
        }

        protected State Wait_Update()
        {
            float timeRemaining = Time.time - startTime;
            if (timeRemaining > duration)
            {
                return State.Success;
            }

            return State.Running;
        }
    }
}