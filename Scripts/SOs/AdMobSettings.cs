using UnityEngine;

[CreateAssetMenu(fileName = "AdMobSettings", menuName = "AdMob/AdMob Settings")]
public class AdMobSettings : ScriptableObject
{
    public enum Environment { Test, Production }

    [Header("Environment Settings")]
    public Environment currentEnvironment = Environment.Test;
    
    [Header("Android IDs")]
    public string androidAppID = "ca-app-pub-3940256099942544~3347511713"; // Test ID
    public string androidBannerID = "ca-app-pub-3940256099942544/6300978111"; // Test ID
    public string androidRewardedID = "ca-app-pub-3940256099942544/5224354917"; // Test ID
    
    [Header("Production IDs (Replace with your real IDs)")]
    public string productionAndroidAppID = "ca-app-pub-2142705696944839~7492180810";
    public string productionAndroidBannerID = "ca-app-pub-2142705696944839/1697778713";
    public string productionAndroidRewardedID = "ca-app-pub-2142705696944839/2819288699";

    // Property accessors
    public string AppID => currentEnvironment == Environment.Test ? androidAppID : productionAndroidAppID;
    public string BannerID => currentEnvironment == Environment.Test ? androidBannerID : productionAndroidBannerID;
    public string RewardedID => currentEnvironment == Environment.Test ? androidRewardedID : productionAndroidRewardedID;
}