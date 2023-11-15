using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/NPC/Delete")]

public class Hack_NPC_Delete : HackAbility
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
            npc.Hack_Delete();
            
        }

        return 0;
    }
}
