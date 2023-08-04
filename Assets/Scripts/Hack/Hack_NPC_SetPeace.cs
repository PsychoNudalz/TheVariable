namespace Hack
{
    public class Hack_NPC_SetPeace:HackAbility
    {
        protected override void AwakeBehaviour()
        {
            
        }

        protected override void StartBehaviour()
        {
        }

        protected override void UpdateBehaviour()
        {
        }

        public override int Hack(HackContext hackContext)
        {
            SmartObject current = hackContext.SmartObjects[0];
            if (current is NpcObject npc)
            {
                npc.Hack_SetAlertState(NPC_AlertState.Peace);
            }

            return 0;
        }
    }
}