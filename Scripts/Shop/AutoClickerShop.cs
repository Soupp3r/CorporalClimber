using UnityEngine;
using System.Collections.Generic;

public class AutoClickerShop : MonoBehaviour
{
    [Header("AutoClicker Setup")]
    [Tooltip("List of all AutoClickerSO assets in the game")]
    public List<AutoClickerSO> autoClickers = new();  // Optional (used for save/load or dynamic spawning)

    [Header("Currency Holder")]
    [Tooltip("The MainButton script storing mainButtoncount")]
    public MainButton mainButton;

    [Header("Global Stats")]
    [Tooltip("Sum of all clicker rates purchased")]
    public float TotalClickRate = 0f;

    /// <summary>
    /// Called by each AutoClickerSlot after a successful purchase.
    /// Adds to global click rate.
    /// </summary>
    public void RegisterClickRate(float addAmount)
    {
        TotalClickRate += addAmount;
    }
}
