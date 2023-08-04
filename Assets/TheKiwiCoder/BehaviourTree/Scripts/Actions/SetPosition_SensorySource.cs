namespace TheKiwiCoder
{
    public class SetPosition_SensorySource:ActionNode
    {
        protected override void OnStart()
        {
            blackboard.moveToPosition = context.NpcController.GetCurrentSS.Position;

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