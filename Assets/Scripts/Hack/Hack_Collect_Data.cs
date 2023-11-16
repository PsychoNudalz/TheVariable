using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/General/Collect_Data")]

public class Hack_Collect_Data : HackAbility
{
    [SerializeField]
    private int gb = 100;
    private bool hasCollected = false;

    public int Gb => gb;

    protected override void AwakeBehaviour()
    {
    }

    protected override void StartBehaviour()
    {
        hasCollected = false;
    }

    protected override void UpdateBehaviour()
    {
    }

    public override int Hack(HackContext hackContext)
    {
        hasCollected = true;
        PlayerController.current.AddGB(gb);
        showHack = false;
        return 0;
    }

    public override bool CanHack()
    {
        if (!hasCollected)
        {
            return base.CanHack();
        }

        return false;
    }
}
