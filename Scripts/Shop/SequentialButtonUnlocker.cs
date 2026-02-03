using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SequentialButtonUnlocker : MonoBehaviour
{
    [Header("Button Progression Settings")]
    public List<Button> orderedButtons = new List<Button>();
    public List<AutoClickerSlot> autoClickerSlots = new List<AutoClickerSlot>();

    [Header("Color Settings")]
    public Color disabledColor = Color.gray;
    public Color enabledColor = Color.white;
    public Color affordableColor = Color.green;

    [Header("Save Settings")]
    public string saveKey = "ButtonUnlockIndex";

    private int currentUnlockIndex = 0;

    private void Start()
    {
        currentUnlockIndex = PlayerPrefs.GetInt(saveKey, 0);
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // Set up all buttons
        for (int i = 0; i < orderedButtons.Count; i++)
        {
            int index = i; // Capture for closure
            Button btn = orderedButtons[i];
            
            // Clear existing listeners
            btn.onClick.RemoveAllListeners();
            
            // Add new listener that checks purchase conditions
            btn.onClick.AddListener(() => AttemptPurchase(index));
            
            // Set initial state
            UpdateButtonState(index);
        }
    }

    private void AttemptPurchase(int buttonIndex)
    {
        // Only allow purchase if this is the next in sequence or already unlocked
        if (buttonIndex > currentUnlockIndex) return;

        AutoClickerSlot slot = autoClickerSlots[buttonIndex];
        MainButton mainButton = slot.mainButton;
        
        // Check affordability
        if (mainButton.mainButtoncount >= slot.autoClickerSO.PurchasePrice)
        {
            // Process purchase
            mainButton.mainButtoncount -= slot.autoClickerSO.PurchasePrice;
            slot.autoClickerSO.CurrentClickRate += slot.autoClickerSO.AddClickRate;
            slot.autoClickerSO.PurchasePrice *= 1.2f;
            slot.shop.TotalClickRate += slot.autoClickerSO.AddClickRate;

            // Unlock next button if this was the current unlock index
            if (buttonIndex == currentUnlockIndex)
            {
                currentUnlockIndex++;
                PlayerPrefs.SetInt(saveKey, currentUnlockIndex);
                PlayerPrefs.Save();
                
                // Update all button states
                for (int i = 0; i < orderedButtons.Count; i++)
                {
                    UpdateButtonState(i);
                }
            }
        }
    }

    private void UpdateButtonState(int buttonIndex)
    {
        Button btn = orderedButtons[buttonIndex];
        AutoClickerSlot slot = autoClickerSlots[buttonIndex];
        
        if (buttonIndex <= currentUnlockIndex)
        {
            // Button is unlocked
            btn.interactable = true;
            
            // Check if affordable
            bool canAfford = slot.mainButton.mainButtoncount >= slot.autoClickerSO.PurchasePrice;
            SetButtonImagesColor(btn, canAfford ? affordableColor : enabledColor);
        }
        else
        {
            // Button is locked
            btn.interactable = false;
            SetButtonImagesColor(btn, disabledColor);
        }
    }

    private void SetButtonImagesColor(Button button, Color color)
    {
        Image[] images = button.GetComponentsInChildren<Image>(true);
        foreach (var img in images)
        {
            img.color = color;
        }
    }

    private void Update()
    {
        // Update button states based on affordability
        for (int i = 0; i <= currentUnlockIndex && i < orderedButtons.Count; i++)
        {
            UpdateButtonState(i);
        }
    }
}