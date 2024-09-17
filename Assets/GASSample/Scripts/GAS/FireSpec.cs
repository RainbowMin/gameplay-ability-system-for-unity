using GAS.Runtime;
using UnityEngine;

public class FireSpec : AbilitySpec<Fire>
{
    public FireSpec(Fire ability, AbilitySystemComponent owner) : base(ability, owner)
    {
    }

    public override void ActivateAbility(params object[] args)
    {
        // 生成子弹
        var bullet = Object.Instantiate(Data.bulletPrefab).GetComponent<Bullet>();
        var transform = Owner.transform;
        bullet.Init(transform.position, transform.up, 10, Owner.AttrSet<AS_Fight>().Atk.CurrentValue);
        TryEndAbility();
    }

    public override void CancelAbility()
    {
    }

    public override void EndAbility()
    {
    }
}