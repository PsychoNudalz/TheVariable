using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Increase player clearance level
/// </summary>
[CreateAssetMenu(menuName = "Hacks/Environment/Clearance Level")]
public class Hack_ClearanceLevel : HackAbility
{
    [Header("Clearance Level")]
    [SerializeField]
    private int clearanceLevel = 0;
    private bool hasCollected = false;


    public int ClearanceLevel => clearanceLevel;

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
        PlayerController.current.IncreaseClearanceLevel(clearanceLevel);
        hasCollected = true;
        showHack = false;
        hackContext.SmartObjects[0].Hack_ChangeMaterial();
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
