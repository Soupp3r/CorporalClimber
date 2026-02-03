using UnityEngine;
using UnityEngine.UI;

public class SoldierPurchasable : MonoBehaviour
{
    public MainButton SO;
    public Button purchaseButton; // Reference to the actual button

    private float amtpurchased = 1f;
    
    private bool wasJustEnabled = false;

    private void Start()
    {
        amtpurchased = PlayerPrefs.GetFloat("amtpurchased", 1f);
    }
    
    void OnEnable()
    {
        wasJustEnabled = true;
        
        // Small delay to avoid the enable-click issue
        Invoke("ResetJustEnabled", 0.1f);
    }
    
    void ResetJustEnabled()
    {
        wasJustEnabled = false;
    }
    
    public void PurchaseSoldierx10()
    {
        if (wasJustEnabled)
        {
            Debug.Log("Ignoring purchase because button was just enabled");
            wasJustEnabled = false;
            return;
        }
        amtpurchased += 1f;
        
        Debug.Log($"Purchase clicked - Multiplier before: {SO.BoughtMultiplier}");
        if(amtpurchased == 1f)
        {
        SO.BoughtMultiplier *= 10;
        }
        else
        {
            SO.BoughtMultiplier *= amtpurchased;
        }
        Debug.Log($"Purchase completed - Multiplier after: {SO.BoughtMultiplier}");

        PlayerPrefs.SetFloat("amtpurchased", amtpurchased);
    }

}