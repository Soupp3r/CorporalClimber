using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardAdScript : MonoBehaviour
{
    public static RewardAdScript Instance;

    [Header("AdMob Settings (ScriptableObject)")]
    public AdMobSettings settings;   // Drag your AdMobSettings SO here
    public MainButton SO;                // ✅ Reference to your ScriptableObject with mainButtoncount & IncreaseAdRewardAmount

    private RewardedAd rewardedAd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ✅ Initialize Mobile Ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads initialized.");
            LoadRewardedAd();
        });
    }

    /// <summary>
    /// Loads a rewarded ad
    /// </summary>
    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading Rewarded Ad...");

        var adRequest = new AdRequest();

        RewardedAd.Load(settings.RewardedID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            rewardedAd = ad;

            // Subscribe to ad events
            rewardedAd.OnAdFullScreenContentOpened += () => Debug.Log("Rewarded ad opened.");
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed.");
                LoadRewardedAd(); // Auto reload next ad
            };
            rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to show: " + error);
                LoadRewardedAd();
            };
            rewardedAd.OnAdImpressionRecorded += () => Debug.Log("Rewarded ad impression recorded.");
            rewardedAd.OnAdClicked += () => Debug.Log("Rewarded ad clicked.");
            rewardedAd.OnAdPaid += (AdValue adValue) => Debug.Log($"Rewarded ad paid: {adValue.Value} {adValue.CurrencyCode}");
        });
    }

    /// <summary>
    /// Shows the rewarded ad
    /// </summary>
    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // ✅ Custom reward logic from your SO
                SO.mainButtoncount += (100f + SO.IncreaseAdRewardAmount);
                Debug.Log($"Player rewarded: +{100f + SO.IncreaseAdRewardAmount} (mainButtoncount now {SO.mainButtoncount})");
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready yet.");
        }
    }
}
