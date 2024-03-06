///////////////////////////////////
//// This is a generated file. ////
////     Do not modify it.     ////
///////////////////////////////////
using System;
using System.Collections.Generic;

namespace GAS.Runtime.Ability
{
  public static class AbilityCollection
  {
      public struct AbilityInfo
      {
          public string Name;
          public string AssetPath;
          public Type AbilityClassType;
      }

    public static AbilityInfo BossAttack01_Info = new AbilityInfo { Name = "BossAttack01", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Boss/BossAttack01.asset",AbilityClassType = typeof(GAS.Runtime.Ability.TimelineAbility.TimelineAbility) };

    public static AbilityInfo Jump_Info = new AbilityInfo { Name = "Jump", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Jump.asset",AbilityClassType = typeof(GAS.Runtime.Ability.Jump) };

    public static AbilityInfo Move_Info = new AbilityInfo { Name = "Move", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Move.asset",AbilityClassType = typeof(GAS.Runtime.Ability.Move) };

    public static AbilityInfo Attack_Info = new AbilityInfo { Name = "Attack", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Player/Attack.asset",AbilityClassType = typeof(GAS.Runtime.Ability.TimelineAbility.TimelineAbility) };

    public static AbilityInfo Defend_Info = new AbilityInfo { Name = "Defend", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Player/Defend.asset",AbilityClassType = typeof(GAS.Runtime.Ability.TimelineAbility.TimelineAbility) };

    public static AbilityInfo DodgeStep_Info = new AbilityInfo { Name = "DodgeStep", AssetPath = "Assets/Demo/Resources/GAS_Setting/Config/GameplayAbilityLib/Player/DodgeStep.asset",AbilityClassType = typeof(GAS.Runtime.Ability.TimelineAbility.TimelineAbility) };

  public static Dictionary<string, AbilityInfo> AbilityMap = new Dictionary<string, AbilityInfo>
  {
      ["BossAttack01"] = BossAttack01_Info,
      ["Jump"] = Jump_Info,
      ["Move"] = Move_Info,
      ["Attack"] = Attack_Info,
      ["Defend"] = Defend_Info,
      ["DodgeStep"] = DodgeStep_Info,
  };
  }
}
