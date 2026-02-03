using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private AdMobSettings settings; // assign in Inspector

    private BannerView bannerView;
    private RewardedAd rewardedAd;

    private void Awake()
    {
        // Singleton pattern + persist across scenes
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
        // Initialize with environment-specific AppID
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log($"AdMob Initialized with AppID: {settings.AppID}");
        });

        // Preload rewarded ad at start
        LoadRewardedAd();
    }

    // -----------------------------
    // Banner Ads
    // -----------------------------
    public void RequestBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(settings.BannerID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();

        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner loaded successfully.");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner failed to load: " + error);
        };

        bannerView.LoadAd(request);
    }

    // -----------------------------
    // Rewarded Ads
    // -----------------------------
    private void LoadRewardedAd()
    {
        AdRequest request = new AdRequest();

        RewardedAd.Load(settings.RewardedID, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            rewardedAd = ad;

            // Events
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed, reloading...");
                LoadRewardedAd(); // reload on close
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Rewarded ad failed to show: " + adError);
            };
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Type}, amount: {reward.Amount}");
                // Grant reward logic here
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready.");
        }
    }
}
