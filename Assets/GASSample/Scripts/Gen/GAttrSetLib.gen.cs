///////////////////////////////////
//// This is a generated file. ////
////     Do not modify it.     ////
///////////////////////////////////

using System;
using System.Collections.Generic;

namespace GAS.Runtime
{
    public class AS_Bullet : AttributeSet
    {
        #region Atk

        /// <summary>
        /// 
        /// </summary>
        public AttributeBase Atk { get; } = new ("AS_Bullet", "Atk", 0f, CalculateMode.Stacking, (SupportedOperation)31, float.MinValue, float.MaxValue);

        public void InitAtk(float value)
        {
            Atk.SetBaseValue(value);
            Atk.SetCurrentValue(value);
        }

        public void SetCurrentAtk(float value)
        {
            Atk.SetCurrentValue(value);
        }

        public void SetBaseAtk(float value)
        {
            Atk.SetBaseValue(value);
        }

        public void SetMinAtk(float value)
        {
            Atk.SetMinValue(value);
        }

        public void SetMaxAtk(float value)
        {
            Atk.SetMaxValue(value);
        }

        public void SetMinMaxAtk(float min, float max)
        {
            Atk.SetMinMaxValue(min, max);
        }

        #endregion Atk

        public override AttributeBase this[string key]
        {
            get
            {
                switch (key)
                {
                    case "Atk":
                        return Atk;
                }

                return null;
            }
        }

        public override string[] AttributeNames { get; } =
        {
            "Atk",
        };

        public override void SetOwner(AbilitySystemComponent owner)
        {
            _owner = owner;
            Atk.SetOwner(owner);
        }

        public static class Lookup
        {
            public const string Atk = "AS_Bullet.Atk";
        }
    }

    public class AS_Fight : AttributeSet
    {
        #region Atk

        /// <summary>
        /// 
        /// </summary>
        public AttributeBase Atk { get; } = new ("AS_Fight", "Atk", 0f, CalculateMode.Stacking, (SupportedOperation)31, float.MinValue, float.MaxValue);

        public void InitAtk(float value)
        {
            Atk.SetBaseValue(value);
            Atk.SetCurrentValue(value);
        }

        public void SetCurrentAtk(float value)
        {
            Atk.SetCurrentValue(value);
        }

        public void SetBaseAtk(float value)
        {
            Atk.SetBaseValue(value);
        }

        public void SetMinAtk(float value)
        {
            Atk.SetMinValue(value);
        }

        public void SetMaxAtk(float value)
        {
            Atk.SetMaxValue(value);
        }

        public void SetMinMaxAtk(float min, float max)
        {
            Atk.SetMinMaxValue(min, max);
        }

        #endregion Atk

        #region HP

        /// <summary>
        /// 
        /// </summary>
        public AttributeBase HP { get; } = new ("AS_Fight", "HP", 0f, CalculateMode.Stacking, (SupportedOperation)31, float.MinValue, float.MaxValue);

        public void InitHP(float value)
        {
            HP.SetBaseValue(value);
            HP.SetCurrentValue(value);
        }

        public void SetCurrentHP(float value)
        {
            HP.SetCurrentValue(value);
        }

        public void SetBaseHP(float value)
        {
            HP.SetBaseValue(value);
        }

        public void SetMinHP(float value)
        {
            HP.SetMinValue(value);
        }

        public void SetMaxHP(float value)
        {
            HP.SetMaxValue(value);
        }

        public void SetMinMaxHP(float min, float max)
        {
            HP.SetMinMaxValue(min, max);
        }

        #endregion HP

        #region Speed

        /// <summary>
        /// 
        /// </summary>
        public AttributeBase Speed { get; } = new ("AS_Fight", "Speed", 0f, CalculateMode.Stacking, (SupportedOperation)31, float.MinValue, float.MaxValue);

        public void InitSpeed(float value)
        {
            Speed.SetBaseValue(value);
            Speed.SetCurrentValue(value);
        }

        public void SetCurrentSpeed(float value)
        {
            Speed.SetCurrentValue(value);
        }

        public void SetBaseSpeed(float value)
        {
            Speed.SetBaseValue(value);
        }

        public void SetMinSpeed(float value)
        {
            Speed.SetMinValue(value);
        }

        public void SetMaxSpeed(float value)
        {
            Speed.SetMaxValue(value);
        }

        public void SetMinMaxSpeed(float min, float max)
        {
            Speed.SetMinMaxValue(min, max);
        }

        #endregion Speed

        public override AttributeBase this[string key]
        {
            get
            {
                switch (key)
                {
                    case "HP":
                        return HP;
                    case "Speed":
                        return Speed;
                    case "Atk":
                        return Atk;
                }

                return null;
            }
        }

        public override string[] AttributeNames { get; } =
        {
            "HP",
            "Speed",
            "Atk",
        };

        public override void SetOwner(AbilitySystemComponent owner)
        {
            _owner = owner;
            HP.SetOwner(owner);
            Speed.SetOwner(owner);
            Atk.SetOwner(owner);
        }

        public static class Lookup
        {
            public const string HP = "AS_Fight.HP";
            public const string Speed = "AS_Fight.Speed";
            public const string Atk = "AS_Fight.Atk";
        }
    }

    public static class GAttrSetLib
    {
        public static readonly Dictionary<string, Type> AttrSetTypeDict = new Dictionary<string, Type>()
        {
            { "Fight", typeof(AS_Fight) },
            { "Bullet", typeof(AS_Bullet) },
        };

        public static readonly Dictionary<Type, string> TypeToName = new Dictionary<Type, string>
        {
            { typeof(AS_Fight), nameof(AS_Fight) },
            { typeof(AS_Bullet), nameof(AS_Bullet) },
        };

        public static List<string> AttributeFullNames = new List<string>()
        {
            "AS_Fight.HP",
            "AS_Fight.Speed",
            "AS_Fight.Atk",
            "AS_Bullet.Atk",
        };
    }
}