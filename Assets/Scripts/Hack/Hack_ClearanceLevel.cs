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

    public int ClearanceLevel => clearanceLevel;

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
        PlayerController.current.IncreaseClearanceLevel(clearanceLevel);
        return 0;
    }
}
