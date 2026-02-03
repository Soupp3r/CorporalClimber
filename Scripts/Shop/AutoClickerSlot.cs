using UnityEngine;
using TMPro;
using System;

public class AutoClickerSlot : MonoBehaviour
{
    [Header("Data")]
    public AutoClickerSO autoClickerSO;

    [Header("UI")]
    public TMP_Text priceText;
    public TMP_Text clickRateText;

    [Header("References")]
    public AutoClickerShop shop;       // Global shop (for total click rate)
    public MainButton mainButton;      // To subtract currency

    public event Action OnPurchase;

    void Start()
    {
        UpdateUI();
    }

    public void Buy()
    {
        if (mainButton.mainButtoncount < autoClickerSO.PurchasePrice)
        {
            Debug.Log("Not enough to buy " + autoClickerSO.PurchaseID);
            return;
        }

        mainButton.mainButtoncount -= autoClickerSO.PurchasePrice;
        autoClickerSO.CurrentClickRate += autoClickerSO.AddClickRate;
        autoClickerSO.PurchasePrice *= 1.2f;
        shop.TotalClickRate += autoClickerSO.AddClickRate;

        OnPurchase?.Invoke();

        UpdateUI();
    }

    public void UpdateUI()
    {
        priceText.text = autoClickerSO.PurchasePrice.ToString("0");
        clickRateText.text = shop.TotalClickRate.ToString("0.##");
    }

    void Update()
    {
        UpdateUI(); // Optional: remove this if you only want manual updates
    }
}
