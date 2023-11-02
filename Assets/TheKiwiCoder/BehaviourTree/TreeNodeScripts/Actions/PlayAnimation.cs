using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Playables;

[System.Serializable]
public class PlayAnimation : ActionNode
{
    public NpcAnimation animation;

    public enum PlayAnimationOrientation
    {
        None,
        Interact,
        SensorySource,
        CameraToInvestigate
    }

    public PlayAnimationOrientation orientation = PlayAnimationOrientation.None;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        try
        {
            switch (orientation)
            {
                case PlayAnimationOrientation.None:
                    context.NpcController.PlayAnimation(animation);

                    break;
                case PlayAnimationOrientation.Interact:
                    // context.NpcController.MoveTransform();
                    context.NpcController.PlayAnimation(animation);

                    break;
                case PlayAnimationOrientation.CameraToInvestigate:
                    if (blackboard.cameraToInvestigate)
                    {
                        context.NpcController.MoveTransform(blackboard.cameraToInvestigate.ConnectedSo.InteractPosition,
                            blackboard.cameraToInvestigate.ConnectedSo.InteractRotation, animation);
                    }
                    else
                    {
                        Debug.LogError("Missing camera to investigate");
                        context.NpcController.PlayAnimation(animation);
                    }

                    break;

                case PlayAnimationOrientation.SensorySource:
                    SensorySource ss = blackboard.currentSensorySource;
                    if (ss != null)
                    {
                        if (ss.SmartObject)
                        {
                            if (ss.SmartObject is NpcObject npcObject)
                            {
                                Vector3 dir = (npcObject.Position - agent_Position).normalized;
                                context.NpcController.MoveTransform(agent_Position,
                                    Quaternion.AngleAxis(Vector2.Angle(new Vector2(0, 1f), new Vector2(dir.x, dir.z)),
                                        Vector3.up), animation);
                            }
                            else
                            {
                                context.NpcController.MoveTransform(agent_Position,
                                    ss.SmartObject.InteractRotation, animation);
                            }
                        }
                        else
                        {
                            context.NpcController.MoveTransform(agent_Position, animation);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return State.Success;
        }

        return State.Success;
    }
}