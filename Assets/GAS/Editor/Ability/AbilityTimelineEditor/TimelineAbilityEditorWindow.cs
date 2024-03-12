﻿#if UNITY_EDITOR
namespace GAS.Editor.Ability
{
    using GAS.Editor.Ability.AbilityTimelineEditor;
    using GAS.Runtime.Ability.TimelineAbility;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(TimelineAbilityAsset))]
    public class TimelineAbilityEditorWindow : OdinEditor
    {
        private TimelineAbilityAsset _asset => target as TimelineAbilityAsset;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            if (GUILayout.Button("查看/编辑能力时间轴", GUILayout.Height(30), GUILayout.Width(300))) EditAbilityTimeline();
            EditorGUILayout.EndVertical();
        }

        private void EditAbilityTimeline()
        {
            AbilityTimelineEditorWindow.ShowWindow(_asset);
        }
    }
}
#endif