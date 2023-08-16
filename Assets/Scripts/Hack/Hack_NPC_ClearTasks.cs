using UnityEngine;

[CreateAssetMenu(menuName = "Hacks/NPC/Clear Task")]

public class Hack_NPC_ClearTasks : HackAbility
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hackContext"></param>
    /// <returns></returns>
    public override int Hack(HackContext hackContext)
    {
        if (hackContext.SmartObjects[0] is NpcObject npcObject)
        {
            npcObject.Hack_ClearTasks();
            return 0;
        }

        return 1;
    }
}