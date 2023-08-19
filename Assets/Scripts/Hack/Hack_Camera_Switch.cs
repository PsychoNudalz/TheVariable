using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (!CanHack())
        {
            return -1;
        }
        this.context = hackContext;
        //implementation of the hack
        if (context.SmartObjects[0] is CameraObject co)
        {
            if (hackContext.HackContextEnum is {Length: > 0} &&
                hackContext.HackContextEnum.Contains(HackContext_Enum.Camera_notPushToStack))
            {
                PlayerController.current.ChangeCamera(co, false);
            }
            else
            {
                PlayerController.current.ChangeCamera(co);
            }
        }
        else
        {
            Debug.LogError("Camera switch target is not a camera!");
            return 1;

        }

        return 0;
    }

    public override bool CanHack()
    {
        if (smartObject is CameraObject co)
        {
            return !co.IsLocked;
        }
        
        return base.CanHack();
    }
}