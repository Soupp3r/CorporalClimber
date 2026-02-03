using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "AutoClickerSO", menuName = "Scriptable Objects/AutoClickerSO")]
public class AutoClickerSO : ScriptableObject
{
    [Tooltip("Original price of AutoClicker before upgrades")]
    public float OriginalPrice;

    [Tooltip("Price increases by 20% every purchase")]
    public float PurchasePrice;

    [Tooltip("Click rate added every purchase")]
    public float AddClickRate;

    [Tooltip("Current click rate of this auto clicker")]
    public float CurrentClickRate;

    [Tooltip("ID Given for Items")]
    public string PurchaseID;

    public void ResetValues()
    {
        PurchasePrice    = OriginalPrice;
        CurrentClickRate = 0f;
    }
}
