#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AdMobEditorTool : EditorWindow
{
    private AdMobSettings adMobSettings;

    [MenuItem("Tools/AdMob/Environment Settings")]
    public static void ShowWindow()
    {
        GetWindow<AdMobEditorTool>("AdMob Environment");
    }

    void OnGUI()
    {
        adMobSettings = EditorGUILayout.ObjectField("AdMob Settings", adMobSettings, typeof(AdMobSettings), false) as AdMobSettings;

        if (adMobSettings == null)
        {
            EditorGUILayout.HelpBox("Assign an AdMobSettings ScriptableObject", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Environment", EditorStyles.boldLabel);
        adMobSettings.currentEnvironment = (AdMobSettings.Environment)EditorGUILayout.EnumPopup(adMobSettings.currentEnvironment);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Information", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"App ID: {adMobSettings.AppID}");
        EditorGUILayout.LabelField($"Banner ID: {adMobSettings.BannerID}");
        EditorGUILayout.LabelField($"Rewarded ID: {adMobSettings.RewardedID}");

        EditorGUILayout.Space();
        if (GUILayout.Button("Apply Environment Settings"))
        {
            ApplyEnvironmentSettings();
            Debug.Log($"AdMob environment set to: {adMobSettings.currentEnvironment}");
        }

        EditorGUILayout.HelpBox(
            "Remember to switch to Production before building your release!\n" +
            "Test IDs will not generate revenue.", 
            MessageType.Info);
    }

    private void ApplyEnvironmentSettings()
    {
        EditorUtility.SetDirty(adMobSettings);
        AssetDatabase.SaveAssets();

        // Updated to use FindObjectsByType with FindObjectsInactive.Include and FindObjectsSortMode.None
        var rewardAds = FindObjectsByType<RewardAdScript>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var ad in rewardAds)
        {
            Debug.Log($"{ad.GetType().Name} has been set to {adMobSettings.currentEnvironment} mode");
            EditorUtility.SetDirty(ad);
        }

        var offlineRewards = FindObjectsByType<OfflineRewardSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var ad in offlineRewards)
        {
            Debug.Log($"{ad.GetType().Name} has been set to {adMobSettings.currentEnvironment} mode");
            EditorUtility.SetDirty(ad);
        }

        AssetDatabase.Refresh();
    }
}
#endif