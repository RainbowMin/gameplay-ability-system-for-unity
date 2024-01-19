﻿#if UNITY_EDITOR
namespace GAS.Editor.GameplayAbilitySystem
{
    using GAS.Core;
    using UnityEditor;
    using UnityEngine;

    //[CustomEditor(typeof(GASSettingAsset))]
    public class GASSettingAssetEditor : UnityEditor.Editor
    {
        private GASSettingAsset Asset => (GASSettingAsset)target;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(200));
            EditorGUILayout.LabelField($"Version: {GasDefine.GAS_VERSION}", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            Asset.GASConfigAssetPath =
                EditorGUILayout.TextField("Config Asset Path", Asset.GASConfigAssetPath);

            GUILayout.Space(5f);

            Asset.CodeGeneratePath =
                EditorGUILayout.TextField("Code Gen Path", Asset.CodeGeneratePath);
            
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("<size=12><color=white>This is the Code Of Method:LoadAbilityAsset. \n" +
                            "For the convenience to access the abilities,you need a static class for the ability assets.\n" +
                            "<color=orange>Therefore,you should determine a method for load ability assets.\n" +
                            "Confirm the method valid<b>(full namespace, '{0}' for the replacement of the path of the ability asset)</b></color></color></size>"
               ,new GUIStyle(){richText = true});
            EditorGUILayout.EndVertical();
            
            Asset.StringCodeOfLoadAbilityAsset =
                EditorGUILayout.TextField("String Code Of Method:LoadAbilityAsset ", Asset.StringCodeOfLoadAbilityAsset);
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(10f);


            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Gameplay Local Lib", EditorStyles.boldLabel);
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("AbilitySystemComponent Lib Path:  " + GASSettingAsset.ASCLibPath);
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Gameplay Ability Lib Path:  " + GASSettingAsset.GameplayAbilityLibPath);
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Gameplay Effect Lib Path:  " + GASSettingAsset.GameplayEffectLibPath);
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("Gameplay Cue Lib Path:" + GASSettingAsset.GameplayCueLibPath);
            GUILayout.Space(3f);
            EditorGUILayout.LabelField("MMC Lib Path:  " + GASSettingAsset.MMCLibPath);

            EditorGUILayout.EndVertical();

            GUILayout.Space(20f);

            if (GUILayout.Button("Save")) Save();

            EditorGUILayout.EndVertical();
        }

        private void Save()
        {
            CheckAllPathFolderExist();
            EditorUtility.SetDirty(Asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void CheckPathFolderExist(string folderPath)
        {
            var folders = folderPath.Split('/');
            if (folders[0] != "Assets")
            {
                EditorUtility.DisplayDialog("Error!", "'Config Asset Path/Code Gen Path' must start with Assets!",
                    "OK");
                return;
            }

            string parentFolderPath = folders[0];
            for (var i = 1; i < folders.Length; i++)
            {
                string newFolderName = folders[i];
                if (newFolderName == "") continue;

                string newFolderPath = parentFolderPath + "/" + newFolderName;
                if (!AssetDatabase.IsValidFolder(newFolderPath))
                {
                    AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
                    Debug.Log("[EX] Folder created at path: " + newFolderPath);
                }

                parentFolderPath += "/" + newFolderName;
            }
        }

        void CheckAllPathFolderExist()
        {
            GasDefine.CheckGasAssetFolder();
            CheckPathFolderExist(Asset.GASConfigAssetPath);
            CheckPathFolderExist(Asset.CodeGeneratePath);
            CheckPathFolderExist(GASSettingAsset.ASCLibPath);
            CheckPathFolderExist(GASSettingAsset.GameplayAbilityLibPath);
            CheckPathFolderExist(GASSettingAsset.GameplayEffectLibPath);
            CheckPathFolderExist(GASSettingAsset.GameplayCueLibPath);
            CheckPathFolderExist(GASSettingAsset.MMCLibPath);
        }
    }
}
#endif