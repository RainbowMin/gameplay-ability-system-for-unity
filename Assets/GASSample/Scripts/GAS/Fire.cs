using GAS.Runtime;
using UnityEngine;

public class Fire : AbstractAbility<FireAsset>
{
    // 这里的AbilityAsset是FireAsset类的变量。由abilityAsset转化而来。
    // AbilityAsset在AbstractAbility<T> 中定义。
    public GameObject bulletPrefab => AbilityAsset.bulletPrefab;

    public Fire(FireAsset abilityAsset) : base(abilityAsset)
    {
    }

    public override AbilitySpec CreateSpec(AbilitySystemComponent owner)
    {
        return new FireSpec(this, owner); // 对应下文Fire的AbilitySpec 
    }
}