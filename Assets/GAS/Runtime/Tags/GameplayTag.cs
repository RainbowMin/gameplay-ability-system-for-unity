using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.TreeViewExamples;
using UnityEngine;
using UnityEngine.Serialization;

namespace GAS.Runtime.Tags
{
    [Serializable]
    public struct GameplayTag
    {
        [SerializeField]private string _name;
        [SerializeField]private int _hashCode;
        [SerializeField]private string _shortName;
        [SerializeField]private int[] _ancestorHashCodes;
        [SerializeField]private string[] _ancestorNames;
        
        [SerializeField]private List<int> _descendants;

        public GameplayTag(string name)
        {
            _name = name;
            _hashCode = name.GetHashCode();


            var tags = name.Split('.');
            if (tags.Length > GasDefine.GAS_TAG_MAX_GENERATIONS)
            {
                throw new Exception($"GameplayTag {name} has more than {GasDefine.GAS_TAG_MAX_GENERATIONS} generations");
            }

            _ancestorNames = new string[tags.Length - 1];
            _ancestorHashCodes = new int[tags.Length-1];
            int i = 0;
            string ancestorTag = "";
            while (i < tags.Length - 1)
            {
                ancestorTag += tags[i];
                _ancestorHashCodes[i] = ancestorTag.GetHashCode();
                _ancestorNames[i] = ancestorTag;
                ancestorTag += ".";
                i++;
            }

            _descendants = new List<int>();
            _shortName = tags.Last();
        }

        /// <summary>
        /// Only For Show. 
        /// </summary>
        public string Name => _name;

        /// <summary>
        ///  Only For Show.
        /// </summary>
        public string ShortName => _shortName;

        /// <summary>
        /// Actually ,Use the hash code for compare.
        /// </summary>
        public int HashCode => _hashCode;
        
        public string[] AncestorNames => _ancestorNames;
      
        public bool Root => _ancestorHashCodes.Length == 0;

        public int[] AncestorHashCodes => _ancestorHashCodes;
        
        /// <summary>
        /// Cache for quick searching.
        /// </summary>
        /// <param name="descendant"></param>
        public void AddDescendant(GameplayTag descendant)
        {
            _descendants.Add(descendant.HashCode);
        }        
        
        public void ClearDescendants()
        {
            _descendants.Clear();
        }
        
        public bool IsDescendantOf(GameplayTag other)
        {
            return other._ancestorHashCodes.Contains(HashCode);
        }

        public override bool Equals(object obj)
        {
            return obj is GameplayTag tag && this == tag;
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public static bool operator ==(GameplayTag x, GameplayTag y)
        {
            return x.HashCode == y.HashCode;
        }

        public static bool operator !=(GameplayTag x, GameplayTag y)
        {
            return x.HashCode != y.HashCode;
        }
    }
}