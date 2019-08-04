using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
public class CameraLock : CinemachineExtension
{
    public float YpositionToLock;
    public float XpositionToLock;
    public bool isHorizontalLevel = true;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;

            if (isHorizontalLevel)
            {
                pos.y = YpositionToLock;
            }
            else
            {
                pos.x = XpositionToLock;
            }

            state.RawPosition = pos;
        }
    }
}