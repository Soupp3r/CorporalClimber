using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using GoogleMobileAds.Api;

    public class OfflineRewardSystem : MonoBehaviour
    {
        [Header("AutoClicker Shop Reference")]
        public AutoClickerShop autoClickerShop;

        [Header("Main Currency Scriptable Object")]
        public MainButton mainButton;

        [Header("Offline Reward Settings")]
        public float minOfflineSeconds;
        private string saveKey = "OfflineLastSavedTime";
        private double baseReward = 0;

        [Header("UI Elements")]
        public GameObject rewardPopup;
        public TextMeshProUGUI rewardText;
        public Button adButton;
        public Button noAdButton;

        [Header("Ticket Cost System")]
        public TicketCount ticketData;
        public TextMeshProUGUI ticketCostText;
        public AudioSource successSound;
        public AudioSource failSound;

        [Header("Optional Multiplier Display")]
        public TextMeshProUGUI multiplierText;

        [Header("AdMob Settings")]
        public AdMobSettings adMobSettings; // Changed from AdMob to AdMobSettings

        private float currentTicketCost = 20f;
        private float offlineMultiplierPercent = 0f;
        private RewardedAd rewardedAd;

        private const string costKey = "OfflineTicketCost";
        private const string multiplierKey = "OfflineMultiplierPercent";

        private void Start()
        {
            if (adMobSettings != null)
    {
        Debug.Log($"Initializing ads in {adMobSettings.currentEnvironment} mode");
    }

            LoadTicketCost();
            LoadMultiplier();
            UpdateTicketCostText();
            UpdateMultiplierText();

            MobileAds.Initialize(initStatus => LoadRewardedAd());
            Invoke(nameof(LoadOfflineTime), 0.1f);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) SaveCurrentTime();
        }

        private void OnApplicationQuit()
        {
            SaveCurrentTime();
        }

        private void SaveCurrentTime()
        {
            PlayerPrefs.SetString(saveKey, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        public void LoadOfflineTime()
        {
            if (!PlayerPrefs.HasKey(saveKey)) return;

            if (!DateTime.TryParse(PlayerPrefs.GetString(saveKey), out DateTime savedTime)) return;

            TimeSpan timeGone = DateTime.Now - savedTime;
            double secondsGone = timeGone.TotalSeconds;

            if (secondsGone >= minOfflineSeconds && autoClickerShop.TotalClickRate > 0f)
            {
                baseReward = secondsGone * autoClickerShop.TotalClickRate * offlineMultiplierPercent / 100f;
                ShowRewardPopup(baseReward);
            }
        }

        private void ShowRewardPopup(double reward)
        {
            rewardText.text = $"You earned {Mathf.FloorToInt((float)reward)} tickets while away!\nWatch an ad to double your reward?";
            rewardPopup.SetActive(true);

            adButton.interactable = rewardedAd != null && rewardedAd.CanShowAd();

            adButton.onClick.RemoveAllListeners();
            noAdButton.onClick.RemoveAllListeners();

            adButton.onClick.AddListener(OnAdRewardButtonClicked);
            noAdButton.onClick.AddListener(() => CollectReward(false));
        }

        private void OnAdRewardButtonClicked()
        {
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show(reward => OnAdComplete(true));
            }
            else
            {
                CollectReward(false);
            }
        }

        private void OnAdComplete(bool completed)
        {
            CollectReward(completed);
        }

        private void CollectReward(bool watchedAd)
        {
            double finalReward = watchedAd ? baseReward * 2 : baseReward;
            mainButton.mainButtoncount += Mathf.FloorToInt((float)finalReward);

            rewardPopup.SetActive(false);
            baseReward = 0;
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
    {
        if (adMobSettings == null)
        {
            Debug.LogError("AdMobSettings reference is missing!");
            return;
        }

        if (string.IsNullOrEmpty(adMobSettings.RewardedID))
        {
            Debug.LogError("Rewarded Ad Unit ID is missing in AdMob ScriptableObject.");
            return;
        }

        var adUnitId = adMobSettings.RewardedID;
        Debug.Log($"Loading rewarded ad with ID: {adUnitId}");
        var request = new AdRequest();

        // THIS IS THE CORRECTED LINE - Changed from RewardedID.Load to RewardedAd.Load
        RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"Failed to load rewarded ad: {error}");
                return;
            }
            rewardedAd = ad;
        });
    }

        public void AttemptPurchase()
        {
            if (ticketData.Ticketcount >= currentTicketCost)
            {
                ticketData.Ticketcount -= currentTicketCost;
                if (successSound) successSound.Play();

                currentTicketCost *= 1.2f;
                offlineMultiplierPercent += 5f;

                SaveTicketCost();
                SaveMultiplier();

                UpdateTicketCostText();
                UpdateMultiplierText();
            }
            else
            {
                if (failSound) failSound.Play();
            }
        }

        private void UpdateTicketCostText()
        {
            if (ticketCostText != null)
                ticketCostText.text = $"{Mathf.CeilToInt(currentTicketCost)}";
        }

        private void UpdateMultiplierText()
        {
            if (multiplierText != null)
                multiplierText.text = $"Offline Bonus: {offlineMultiplierPercent}%";
        }

        private void SaveTicketCost()
        {
            PlayerPrefs.SetFloat(costKey, currentTicketCost);
            PlayerPrefs.Save();
        }

        private void LoadTicketCost()
        {
            currentTicketCost = PlayerPrefs.GetFloat(costKey, 20f);
        }

        private void SaveMultiplier()
        {
            PlayerPrefs.SetFloat(multiplierKey, offlineMultiplierPercent);
            PlayerPrefs.Save();
        }

        private void LoadMultiplier()
        {
            offlineMultiplierPercent = PlayerPrefs.GetFloat(multiplierKey, 0f);
        }
    }
