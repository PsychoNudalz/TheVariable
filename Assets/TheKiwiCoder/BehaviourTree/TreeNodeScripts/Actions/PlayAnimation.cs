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
        CameraToInvestigate,
        CameraToLock
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
                    if (blackboard.CameraToInvestigate)
                    {
                        if (blackboard.CameraToInvestigate.ConnectedSo is NpcObject npcObject)
                        {
                            context.NpcController.MoveTransform(agent_Position,
                                GetDirectionToNpc(npcObject), animation);
                        }
                        else
                        {
                            context.NpcController.MoveTransform(
                                blackboard.CameraToInvestigate.ConnectedSo.InteractPosition,
                                blackboard.CameraToInvestigate.ConnectedSo.InteractRotation, animation);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Missing camera to investigate");
                        context.NpcController.PlayAnimation(animation);
                    }

                    break;
                case PlayAnimationOrientation.CameraToLock:
                    if (blackboard.cameraToLock)
                    {
                        if (blackboard.cameraToLock.ConnectedSo is NpcObject npcObject)
                        {
                            context.NpcController.MoveTransform(agent_Position,
                                GetDirectionToNpc(npcObject), animation);
                        }
                        else
                        {
                            context.NpcController.MoveTransform(blackboard.cameraToLock.ConnectedSo.InteractPosition,
                                blackboard.cameraToLock.ConnectedSo.InteractRotation, animation);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Missing camera to lock");
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
                                context.NpcController.MoveTransform(agent_Position,
                                    GetDirectionToNpc(npcObject), animation);
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