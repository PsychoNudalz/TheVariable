using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hack_General_Distraction : HackAbility
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        context = hackContext;
        Debug.Log($"Distraction from {context.SmartObjects[0]} at {context.SmartObjects[0].Position}");
        return 0;
    }
}
