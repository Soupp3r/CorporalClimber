using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PointCollecter : MonoBehaviour
{
    // References
    // REMOVED: private int CountUpgrade = 100; // This was causing the warning
    public MainButton SO;
    public Button UpgradeEffect;
    public RankUpgradeMenu RUM;
    public List<Sprite> UpgradeList;
    public int currentIndex = 1;
    public Image TargetImage;
    public Image TargetImageinRankUpgrade;
    public Slider ExpSlider;
    public TMP_Text mainCountText;
    public KeepingValues KV;

    // Add these to track base values separately from multiplied values
    private float baseExpMultiplier = 1f;
    private float baseMainMultiplier = 1f;

    void Start()
    {
        // Automatically load all sprites in Resources/Sprites/Upgrades
        UpgradeList = new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/Upgrades"));

        // Set the first sprite
        if (UpgradeList.Count > 0)
        {
            TargetImage.sprite = UpgradeList[0];
            TargetImageinRankUpgrade.sprite = UpgradeList[0];
        }

        // Initialize base values
        baseExpMultiplier = SO.ExpMultiplier;
        baseMainMultiplier = SO.mainMultiplier;
        
        UpdateClicksTillNextLevel();
    }

    void Update()
    {
        // Calculate required experience: 10 clicks * current index
        float expRequired = 10 * currentIndex;
        ExpSlider.maxValue = expRequired;

        // Smoothly interpolate the slider value
        ExpSlider.value = Mathf.Lerp(ExpSlider.value, SO.ExpCount, Time.deltaTime * 5f);

        mainCountText.text = SO.mainButtoncount.ToString("F0");
        
        // Check if we should upgrade (only when clicks reach 0)
        if (SO.ClicksTillNextLevel <= 0 && SO.ExpCount >= expRequired)
        {
            Upgrade();
        }
    }

    // Fixed Upgrade method - now only called when conditions are met
    public void Upgrade()
    {
        // Cap the current index to prevent out of bounds
        if (currentIndex < UpgradeList.Count - 1)
        {
            currentIndex++;
            TargetImage.sprite = UpgradeList[currentIndex];
            TargetImageinRankUpgrade.sprite = UpgradeList[currentIndex];
        }

        // Reset experience and clicks
        SO.ExpCount = 0f;
        
        // Increase base multipliers instead of multiplied values
        baseExpMultiplier += 0.25f;
        baseMainMultiplier += 0.25f;
        
        // Apply bonus amount to base values (not multiplied values)
        SO.ExpMultiplier = baseExpMultiplier * SO.BonusAmount;
        SO.mainMultiplier = baseMainMultiplier * SO.BonusAmount;

        // Increase prices with reasonable scaling
        RUM.TicketPrice *= 1.1f; // Reduced from 1.2f to prevent runaway growth
        RUM.SoldierPrice *= 1.1f; // Reduced from 1.2f

        // Add points with controlled growth
        SO.mainButtoncount += currentIndex * 100;
        
        // Update clicks needed with proper calculation
        UpdateClicksTillNextLevel();
        
        // Trigger upgrade effect
        UpgradeEffect.onClick.Invoke();
        
        // Controlled ad reward increase
        SO.IncreaseAdRewardAmount += currentIndex * 10; // Linear instead of multiplicative
        
        // Ensure currentIndex doesn't exceed bounds
        if (currentIndex >= UpgradeList.Count)
        {
            currentIndex = UpgradeList.Count - 1;
        }

        KV.SavePref();
    }

    // NEW: Simplified clicks calculation - always 10 clicks per level multiplied by current index
    private void UpdateClicksTillNextLevel()
    {
        // Calculate required experience: 10 clicks * current index
        float expRequired = 10 * currentIndex;
        
        // Calculate how many clicks are needed to reach the required experience
        float remainingExp = expRequired - SO.ExpCount;
        float expPerClick = SO.BoughtMultiplier * SO.BonusAmount;
        
        // Ensure we don't divide by zero
        if (expPerClick <= 0)
        {
            SO.ClicksTillNextLevel = 1;
            return;
        }
        
        SO.ClicksTillNextLevel = Mathf.CeilToInt(remainingExp / expPerClick);
        
        // Ensure it's never negative
        if (SO.ClicksTillNextLevel < 0)
        {
            SO.ClicksTillNextLevel = 0;
        }
    }

    public void PointCollector()
    {
        SO.mainButtoncount += SO.BoughtMultiplier;

        // Calculate experience gained per click
        float expGained = SO.BoughtMultiplier * SO.BonusAmount;
        SO.ExpCount += expGained;
        
        // Update clicks needed based on current experience
        UpdateClicksTillNextLevel();
    }
}