using GAS.General;
using GAS.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class CueBombWarning : GameplayCueDurational
{
    [BoxGroup]
    [LabelText("半径")]
    public float Radius;

    [BoxGroup]
    [LabelText("可视预制体")]
    public GameObject Visualization;
        
    [BoxGroup]
    [LabelText("持续时间(s)")]
    public float Duration = 1;

    public override GameplayCueDurationalSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueBombWarningSpec(this, parameters);
    }
}