using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/General/Extraction")]

public class Hack_Extraction : HackAbility
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
        GameManager.GameWin();
        return 0;
    }

    public override bool CanHack()
    {
        return GameManager.IsVIPDead&&base.CanHack();
    }
}
