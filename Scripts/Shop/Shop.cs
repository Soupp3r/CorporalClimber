using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public MainButton SO;

    public TMP_Text ExpShopText;

    public TMP_Text CountShopText;

    public TMP_Text mainButtonCountText;

    public TMP_Text ExpCountText;

    private void Update()
    {
        ExpShopText.text = SO.BuyEXPMultiplier.ToString("F0");
        CountShopText.text = SO.BuyCOUNTMultiplier.ToString("F0");
        mainButtonCountText.text = SO.mainButtonCountText.ToString("F0") + "%";
        ExpCountText.text = SO.ExpCountText.ToString("F0") + "%";
    }

    public void BuyExpMultiplier()
    {
        if(SO.mainButtoncount >= SO.BuyEXPMultiplier)
        {
            SO.mainButtoncount -=SO.BuyEXPMultiplier;
            SO.BuyEXPMultiplier *= 1.2f;
            SO.BonusAmount *= 1.2f;
            SO.ExpCountText += 5f;
            SO.ClicksTillNextLevel -= (int)(SO.ClicksTillNextLevel * 0.3);
        }
    }

    public void BuyCountMultiplier()
    {
        if(SO.mainButtoncount >= SO.BuyCOUNTMultiplier)
        {
            SO.mainButtoncount -=SO.BuyCOUNTMultiplier;
            SO.BuyCOUNTMultiplier *= 1.2f;
            SO.BoughtMultiplier *= 1.2f;
            SO.mainButtonCountText += 5f;
        }
    }
}
