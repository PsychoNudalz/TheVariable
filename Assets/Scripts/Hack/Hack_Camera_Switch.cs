using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Hacks/Camera/Switch")]

[Serializable]
public class Hack_Camera_Switch : HackAbility
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
    /// Context[0] will be the current camera to switch to
    /// </summary>
    /// <param name="hackContext">0: camera to switch to</param>
    /// <returns></returns>
    public override int Hack(HackContext hackContext)
    {
        
        this.context = hackContext;
        //implementation of the hack
        if (context.SmartObjects[0] is CameraObject co)
        {
            PlayerController.current.ChangeCamera(co);

        }
        else
        {
            Debug.LogError("Camera switch target is not a camera!");
        }
        return 0;
    }
}
