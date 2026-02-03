using UnityEngine;

public class KeepingValues : MonoBehaviour
{
    [Header("Core References")]
    public MainButton        SO;               // holds currency, multipliers, etc.
    public AutoClickerShop   autoClickerShop;  // holds TotalClickRate & list of SOs
    public PointCollecter    pointcollector;   // whatever upgrades UI you track
    public TicketCount TC;
    public TicketShopPointMultipliers TCShop; ///Ticket shop saving
    public RankUpgradeMenu RU;///RankUpgradeMenuSavingThePrices

    ///ClearPlayerPrefersAtStart//
    private const string FirstRunKey = "HasPlayedBefore";

    private void Start()
    {
        CheckFirstRun();
        LoadPref(); // Now it's safe to load
    }

     private void CheckFirstRun()
    {
        // Check the flag. If it doesn't exist, returns 0 (false).
        int hasPlayedBefore = PlayerPrefs.GetInt(FirstRunKey, 0);

        if (hasPlayedBefore == 0)
        {
            Debug.Log("First run detected. Clearing all saved data.");
            PlayerPrefs.DeleteAll();

            // Set the flag so this doesn't happen again
            PlayerPrefs.SetInt(FirstRunKey, 1);
            PlayerPrefs.Save(); // Force save the flag immediately

            // IMPORTANT: DO NOT CALL LoadPref() here.
            // We want the game to use the default values set in the Inspector.
            return;
        }
        else
        {
            // Not the first run, so we will load the saved data normally in LoadPref()
            Debug.Log("Welcome back!");
        }
    }

    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void DividePurchasesAway()
    {
        TC.Ticketcount -= 55f;
        SO.BoughtMultiplier /= 10f;
        if(SO.BoughtMultiplier <= 9f)
        {
            SO.BoughtMultiplier = 1f;
        }
    }

    /* ───────────────────────────────────────── SAVE ───────────────────────────────────────── */
    public void SavePref()
    {
        /* Game-cycle stats */
        PlayerPrefs.SetFloat("AdRewardAmount", SO.IncreaseAdRewardAmount);
        PlayerPrefs.SetFloat("mainMultiplier",      SO.mainMultiplier);
        PlayerPrefs.SetFloat("expMultiplier",       SO.ExpMultiplier);
        PlayerPrefs.SetFloat("expCount",            SO.ExpCount);
        PlayerPrefs.SetFloat("mainCount",           SO.mainButtoncount);
        PlayerPrefs.SetFloat("BonusAmt",            SO.BonusAmount);
        PlayerPrefs.SetFloat("BoughtMultiplier",    SO.BoughtMultiplier);
        PlayerPrefs.SetFloat("BuyEXPMultiplier",    SO.BuyEXPMultiplier);
        PlayerPrefs.SetFloat("BuyCOUNTMultiplier",  SO.BuyCOUNTMultiplier);
        PlayerPrefs.SetInt   ("currentIndex",       pointcollector.currentIndex);
        PlayerPrefs.SetFloat("BuyCountText%", SO.mainButtonCountText);
        PlayerPrefs.SetFloat("ExpCountText%", SO.ExpCountText);
        PlayerPrefs.SetInt("ClicksTillNextRank", SO.ClicksTillNextLevel);
        PlayerPrefs.SetFloat("TicketAmount", TC.Ticketcount);
        PlayerPrefs.SetFloat("MultiplyDrop", TC.MultiplyDrop);
        PlayerPrefs.SetFloat("DropAtNumber", TC.DropAtNumber);

        /* Ticket shop */
        PlayerPrefs.SetFloat("Purchasefloat", TCShop.Purchasefloat);
        PlayerPrefs.SetFloat("MultiplyByFloat", TCShop.MultiplyByFloat);
        PlayerPrefs.SetFloat("ClicksTillActive", TCShop.ClicksTillActive);
        PlayerPrefs.SetFloat("TCShopPurchasePrice", TCShop.PurchasePrice);

        /* Volume */
        PlayerPrefs.SetFloat("MainVolume",   SO.MainVolume);
        PlayerPrefs.SetFloat("MusicVolume",  SO.MusicVolume);
        PlayerPrefs.SetFloat("SfxandUIVolume", SO.SfxandUIVolume);

        /* Every AutoClickerSO in the shop */
        foreach (var clicker in autoClickerShop.autoClickers)
        {
            string id = clicker.PurchaseID;
            PlayerPrefs.SetFloat($"{id}_Price",      clicker.PurchasePrice);
            PlayerPrefs.SetFloat($"{id}_ClickRate",  clicker.CurrentClickRate);
            // If you add clicker.TimesPurchased, also save it here.
        }

        /* Global total click rate */
        PlayerPrefs.SetFloat("TotalClickRate", autoClickerShop.TotalClickRate);

        /*RankUpgradePrices*/
        PlayerPrefs.SetFloat("RUTicketPrice", RU.TicketPrice);
        PlayerPrefs.SetFloat("RUSoldierPrice", RU.SoldierPrice);


        PlayerPrefs.SetInt(FirstRunKey, 1);
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs saved.");
    }

    /* ───────────────────────────────────────── LOAD ───────────────────────────────────────── */
    public void LoadPref()
    {
        //Start//

        

        /* Game-cycle stats */
        TC.Ticketcount = PlayerPrefs.GetFloat("TicketAmount", 0f);
        TC.MultiplyDrop = PlayerPrefs.GetFloat("MultiplyDrop", 1f);
        TC.DropAtNumber = PlayerPrefs.GetFloat("DropAtNumber", 0f);
        SO.IncreaseAdRewardAmount = PlayerPrefs.GetFloat("AdRewardAmount", 100f);
        SO.ClicksTillNextLevel = PlayerPrefs.GetInt("ClicksTillNextRank", 37);
        SO.mainMultiplier      = PlayerPrefs.GetFloat("mainMultiplier", 0.25f);
        SO.ExpMultiplier       = PlayerPrefs.GetFloat("expMultiplier",  0.25f);
        SO.ExpCount            = PlayerPrefs.GetFloat("expCount",       0f);
        SO.mainButtoncount     = PlayerPrefs.GetFloat("mainCount",      0f);
        SO.BonusAmount         = PlayerPrefs.GetFloat("BonusAmt",       2f);
        SO.BoughtMultiplier    = PlayerPrefs.GetFloat("BoughtMultiplier", 1f);
        Debug.Log($"LOADED - BoughtMultiplier: {SO.BoughtMultiplier}, HasKey: {PlayerPrefs.HasKey("BoughtMultiplier")}");
        SO.BuyEXPMultiplier    = PlayerPrefs.GetFloat("BuyEXPMultiplier", 30000f);
        SO.BuyCOUNTMultiplier  = PlayerPrefs.GetFloat("BuyCOUNTMultiplier", 1500f);
        SO.mainButtonCountText = PlayerPrefs.GetFloat("BuyCountText%", 5f);
        SO.ExpCountText = PlayerPrefs.GetFloat("ExpCountText%", 5f);
        pointcollector.currentIndex = PlayerPrefs.GetInt("currentIndex", 0);

        if (pointcollector.UpgradeList.Count > 0 &&
            pointcollector.currentIndex < pointcollector.UpgradeList.Count)
        {
            pointcollector.TargetImage.sprite = pointcollector.UpgradeList[pointcollector.currentIndex];
            pointcollector.TargetImageinRankUpgrade.sprite = pointcollector.UpgradeList[pointcollector.currentIndex];
        }

        /* Ticket Shop */
        TCShop.Purchasefloat = PlayerPrefs.GetFloat("Purchasefloat", 0f);
        TCShop.MultiplyByFloat = PlayerPrefs.GetFloat("MultiplyByFloat", 0f);
        TCShop.ClicksTillActive = PlayerPrefs.GetFloat("ClicksTillActive", 600f);
        TCShop.PurchasePrice = PlayerPrefs.GetFloat("TCShopPurchasePrice", 15f);

        /* Volume */
        SO.MainVolume   = PlayerPrefs.GetFloat("MainVolume",   1f);
        SO.MusicVolume  = PlayerPrefs.GetFloat("MusicVolume",  1f);
        SO.SfxandUIVolume = PlayerPrefs.GetFloat("SfxandUIVolume", 1f);

        /*RankUpgradePrices*/
        RU.TicketPrice = PlayerPrefs.GetFloat("RUTicketPrice", 10f);
        RU.SoldierPrice = PlayerPrefs.GetFloat("RUSoldierPrice", 50000f);

        /* AutoClickers */
        foreach (var clicker in autoClickerShop.autoClickers)
{
    string id = clicker.PurchaseID;

    if (PlayerPrefs.HasKey($"{id}_Price"))
    {
        clicker.PurchasePrice    = PlayerPrefs.GetFloat($"{id}_Price");
        clicker.CurrentClickRate = PlayerPrefs.GetFloat($"{id}_ClickRate");
    }
    else
    {
        // Reset to original if nothing was saved
        clicker.ResetValues();
    }
}

        autoClickerShop.TotalClickRate = PlayerPrefs.GetFloat("TotalClickRate", 0f);

        Debug.Log("PlayerPrefs loaded.");
    }

    /* ────────────────────────────── Auto-save hooks ─────────────────────────────── */
    private void OnApplicationQuit()   => SavePref();
    private void OnApplicationPause(bool pause) { if (pause) SavePref(); }
}
