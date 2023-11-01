using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hacks/Environment/Door_LockOpen")]

public class Hack_Door_LockOpen : HackAbility
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
        if (hackContext.SmartObjects[0] is EnvironmentObject environmentObject)
        {
            environmentObject.Hack_LockOpen();
            return 0;
        }
        else
        {
            Debug.LogError("Hack lock open cant be used on none environmentObject");
            return 1;
        }
        
    }
}
