using GAS.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class CueCameraShake : GameplayCueInstant
{
    [LabelText("晃动强度")]
    public float shakePower = 0.5f;  // 默认晃动强度为0.5

    [LabelText("晃动时长")]
    public float shakeDuration = 0.5f; // 默认晃动时长为0.5s

    public override GameplayCueInstantSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueCameraShakeSpec(this, parameters);
    }
}
