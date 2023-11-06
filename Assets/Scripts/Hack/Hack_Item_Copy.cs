using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/Item/Copy")]

public class Hack_Item_Copy : HackAbility
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
        if (hackContext.SmartObjects[0] is ItemObject itemObject)
        {
            PlayerController.current.CopyItem(itemObject.OriginalItemName);
            return 0;
        }
        else
        {
            return -1;
        }
    }
}
