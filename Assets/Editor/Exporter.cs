using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace Pal
{
    public class Exporter : MonoBehaviour
    {
        [MenuItem("CYC/Export Test")]
        static void Export()
        {

            BuildPipeline.PushAssetDependencies();
            ExportShaders();
            ExportInitial();
            ExportAtlas();
            ExportUI();
            BuildPipeline.PopAssetDependencies();
        }

        [MenuItem("CYC/Export Shader")]
        static void ExportShaders()
        {
            BuildAssetBundleOptions options =
                BuildAssetBundleOptions.CollectDependencies |
                BuildAssetBundleOptions.CompleteAssets |
                BuildAssetBundleOptions.DeterministicAssetBundle;

            BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/Resources/Prefabs/ShaderList.prefab"), null, "CYCData/ShadersList.assetbundle", options);

        }
        static void ExportInitial()
        {
            BuildAssetBundleOptions options =
                BuildAssetBundleOptions.CollectDependencies |
                BuildAssetBundleOptions.CompleteAssets |
                BuildAssetBundleOptions.DeterministicAssetBundle;

            BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/Resources/Prefabs/Initial.prefab"), null, "CYCData/Initial.assetbundle", options);

        }
        [MenuItem("CYC/Export UI")]
        static void ExportUI()
        {
            BuildAssetBundleOptions options =
                BuildAssetBundleOptions.CollectDependencies |
                BuildAssetBundleOptions.CompleteAssets |
                BuildAssetBundleOptions.DeterministicAssetBundle;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("CYCData/UI_FlagPanel.assetbundle","Assets/Resources/UI/FlagPanel.prefab"); 
            dict.Add("CYCData/UI_LabelSpritePanel.assetbundle","Assets/Resources/UI/LabelSpritePanel.prefab"); 
            dict.Add("CYCData/UI_NGUIPanel.assetbundle","Assets/Resources/UI/NGUIPanel.prefab"); 
            foreach(var v in dict)
            {
                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath(v.Value), null, v.Key, options);
                BuildPipeline.PopAssetDependencies();
            }
        }
        [MenuItem("CYC/Export atlas")]
        static void ExportAtlas()
        {
            BuildAssetBundleOptions options =
                BuildAssetBundleOptions.CollectDependencies |
                BuildAssetBundleOptions.CompleteAssets |
                BuildAssetBundleOptions.DeterministicAssetBundle;

            BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/NGUI/Examples/Atlases/Fantasy/Fantasy Atlas.prefab"), null, "CYCData/AT_Fantasy.assetbundle", options);
            BuildPipeline.BuildAssetBundle(AssetDatabase.LoadMainAssetAtPath("Assets/NGUI/Examples/Atlases/Wooden/Wooden Atlas.prefab"), null, "CYCData/AT_Wooden.assetbundle", options);
        }
    }
}
