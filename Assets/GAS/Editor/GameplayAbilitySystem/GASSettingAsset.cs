﻿using System;
using GAS.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace GAS.Editor.GameplayAbilitySystem
{
    public class GASSettingAsset : ScriptableObject
    {
        private const int LABLE_WIDTH = 200;
        private const int SHORT_LABLE_WIDTH = 200;

        private const string TIP_LOAD_METHOD_OF_ABILITY_ASSET =
            "<size=12><color=white>This is the Code Of Method:LoadAbilityAsset. \n" +
            "For the convenience to access the abilities,you need a static class for the ability assets.\n" +
            "<color=orange>Therefore,you should determine a method for load ability assets.\n" +
            "Confirm the method valid<b>(full namespace," +
            " '{0}' for the replacement of the path of the ability asset)</b></color></color></size>";

        private const string TIP_CREATE_FOLDERS = 
            "<color=white><size=15>If you change the path of GAS Asset,please click this button to make sure that all son folders created.</size></color>";

        private static GASSettingAsset _setting;
        
        
        [Title("Setting",Bold = true)]
        [BoxGroup("A", false,order:1)] [LabelText("Code Generate Path")] [LabelWidth(LABLE_WIDTH)]
        [FolderPath]
        public string CodeGeneratePath = "Assets/Scripts/Gen";

        [BoxGroup("A")] [LabelText("GAS Asset Path")] [LabelWidth(LABLE_WIDTH)]
        [FolderPath]
        public string GASConfigAssetPath = "Assets/GAS_Setting/Config";

        [BoxGroup("A")]
        [LabelText("Load Method Of Ability Asset")]
        [LabelWidth(LABLE_WIDTH)]
        [InfoBox(TIP_LOAD_METHOD_OF_ABILITY_ASSET)]
        public string StringCodeOfLoadAbilityAsset = "UnityEngine.Resources.Load<AbilityAsset>({0})";

        public static GASSettingAsset Setting
        {
            get
            {
                if (_setting == null) _setting = Load();

                return _setting;
            }
        }

        [ShowInInspector]
        [BoxGroup("V",false,order:0)]
        [HideLabel][DisplayAsString(TextAlignment.Left,true)]
        private static string Version => $"<size=15><b><color=white>EX-GAS Version: {GasDefine.GAS_VERSION}</color></b></size>";
        
        public static string CodeGenPath => Setting.CodeGeneratePath;


        [Title("Paths")]
        [PropertySpace(10)]
        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        public static string ASCLibPath => $"{Setting.GASConfigAssetPath}/{GasDefine.GAS_ASC_LIBRARY_FOLDER}";

        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        public static string GameplayEffectLibPath =>
            $"{Setting.GASConfigAssetPath}/{GasDefine.GAS_EFFECT_LIBRARY_FOLDER}";

        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        public static string GameplayAbilityLibPath =>
            $"{Setting.GASConfigAssetPath}/{GasDefine.GAS_ABILITY_LIBRARY_FOLDER}";

        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        public static string GameplayCueLibPath => $"{Setting.GASConfigAssetPath}/{GasDefine.GAS_CUE_LIBRARY_FOLDER}";
        
        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        public static string MMCLibPath => $"{Setting.GASConfigAssetPath}/{GasDefine.GAS_MMC_LIBRARY_FOLDER}";

        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        [LabelText("Tag Asset Path")]
        public static string GAS_TAG_ASSET_PATH => $"{Setting.GASConfigAssetPath}/GameplayTagsAsset.asset";

        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left, true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        [LabelText("Attribute Asset Path")]
        public static string GAS_ATTRIBUTE_ASSET_PATH => $"{Setting.GASConfigAssetPath}/AttributeAsset.asset";
        
        [ShowInInspector]
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [LabelWidth(SHORT_LABLE_WIDTH)]
        [LabelText("AttributeSet Asset Path")]
        public static string GAS_ATTRIBUTESET_ASSET_PATH => $"{Setting.GASConfigAssetPath}/AttributeSetAsset.asset";


        private static GASSettingAsset Load()
        {
            var asset = AssetDatabase.LoadAssetAtPath<GASSettingAsset>(GasDefine.GAS_SYSTEM_ASSET_PATH);
            if (asset == null)
            {
                GasDefine.CheckGasAssetFolder();

                var a = CreateInstance<GASSettingAsset>();
                AssetDatabase.CreateAsset(a, GasDefine.GAS_SYSTEM_ASSET_PATH);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                asset = CreateInstance<GASSettingAsset>();
            }

            return asset;
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
        
        [BoxGroup("A")]
        [DisplayAsString(TextAlignment.Left,true)]
        [GUIColor(0,0.8f,0)]
        [PropertySpace(10)]
        [InfoBox(TIP_CREATE_FOLDERS)]
        [Button(SdfIconType.FolderCheck,"Create Folders",ButtonHeight = 38)]
        void CheckAllPathFolderExist()
        {
            GasDefine.CheckGasAssetFolder();
            CheckPathFolderExist(GASConfigAssetPath);
            CheckPathFolderExist(CodeGeneratePath);
            CheckPathFolderExist(ASCLibPath);
            CheckPathFolderExist(GameplayAbilityLibPath);
            CheckPathFolderExist(GameplayEffectLibPath);
            CheckPathFolderExist(GameplayCueLibPath);
            CheckPathFolderExist(MMCLibPath);
        }
    }
}
#endif