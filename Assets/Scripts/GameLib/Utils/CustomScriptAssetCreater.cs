using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class CustomScriptAssetCreater : UnityEditor.Editor
{
    static readonly string effectAssetPath = "Assets/Resources/CustomAssets/Effects";
    [MenuItem("Tools/CreateCustomAssetData/CreateEffectVO")]
    static void CreateEffectVO()
    {
        ScriptableObject effectAsset = ScriptableObject.CreateInstance<EffectAssetData>();

        if (!Directory.Exists(effectAssetPath))
        {
            Directory.CreateDirectory(effectAssetPath);
        }

        string savePath = string.Format("{0}/{1}.asset", effectAssetPath, "EffectAssetData");
        AssetDatabase.CreateAsset(effectAsset, savePath);
    }
}
