namespace TheKiwiCoder
{
    public class Action_ChangeAlertState:ActionNode
    {
        public NPC_AlertState newState;
        protected override void OnStart()
        {
            ChangeAlertState(newState);
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}