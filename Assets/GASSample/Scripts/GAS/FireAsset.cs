using System;
using GAS.Runtime;
using UnityEngine;
public class FireAsset : AbilityAsset
{
    public GameObject bulletPrefab;
    public override Type AbilityType() => typeof(Fire);// 下文对应Fire的Ability    
}


