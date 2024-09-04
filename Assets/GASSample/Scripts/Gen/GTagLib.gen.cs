///////////////////////////////////
//// This is a generated file. ////
////     Do not modify it.     ////
///////////////////////////////////

using System.Collections.Generic;

namespace GAS.Runtime
{
    public static class GTagLib
    {
        public static GameplayTag Ability { get; } = new GameplayTag("Ability");
        public static GameplayTag Ability_Bomb { get; } = new GameplayTag("Ability.Bomb");
        public static GameplayTag Ability_Die { get; } = new GameplayTag("Ability.Die");
        public static GameplayTag Ability_Fire { get; } = new GameplayTag("Ability.Fire");
        public static GameplayTag Ability_Sweep { get; } = new GameplayTag("Ability.Sweep");
        public static GameplayTag CD { get; } = new GameplayTag("CD");
        public static GameplayTag CD_Sweep { get; } = new GameplayTag("CD.Sweep");
        public static GameplayTag Event { get; } = new GameplayTag("Event");
        public static GameplayTag Event_Ban { get; } = new GameplayTag("Event.Ban");
        public static GameplayTag Event_Ban_Move { get; } = new GameplayTag("Event.Ban.Move");
        public static GameplayTag Faction { get; } = new GameplayTag("Faction");
        public static GameplayTag Faction_Enemy { get; } = new GameplayTag("Faction.Enemy");
        public static GameplayTag Faction_Player { get; } = new GameplayTag("Faction.Player");
        public static GameplayTag Moving { get; } = new GameplayTag("Moving");
        public static GameplayTag State { get; } = new GameplayTag("State");
        public static GameplayTag State_Debuff { get; } = new GameplayTag("State.Debuff");
        public static GameplayTag State_Debuff_Cold { get; } = new GameplayTag("State.Debuff.Cold");

        public static Dictionary<string, GameplayTag> TagMap = new Dictionary<string, GameplayTag>
        {
            ["Ability"] = Ability,
            ["Ability.Bomb"] = Ability_Bomb,
            ["Ability.Die"] = Ability_Die,
            ["Ability.Fire"] = Ability_Fire,
            ["Ability.Sweep"] = Ability_Sweep,
            ["CD"] = CD,
            ["CD.Sweep"] = CD_Sweep,
            ["Event"] = Event,
            ["Event.Ban"] = Event_Ban,
            ["Event.Ban.Move"] = Event_Ban_Move,
            ["Faction"] = Faction,
            ["Faction.Enemy"] = Faction_Enemy,
            ["Faction.Player"] = Faction_Player,
            ["Moving"] = Moving,
            ["State"] = State,
            ["State.Debuff"] = State_Debuff,
            ["State.Debuff.Cold"] = State_Debuff_Cold,
        };
    }
}