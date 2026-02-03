using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MainButton", menuName = "Scriptable Objects/MainButton")]
public class MainButton : ScriptableObject
{

    ///Game Needed//
    public float mainButtoncount;
    public float ExpCount;
    public float mainButtonCountText = 5f;
    public float ExpCountText = 5f;
    public float mainMultiplier = 0.25f;
    public float ExpMultiplier = 0.25f;
    public int ClicksTillNextLevel;
    //End//--------------------------------------------------------------------------------

    //For Purchase, /NOTES (BonusAmount going up will change the multiplyer values, 
    // not including Bought multiplier, that will increase the amount added each click)
    public float BonusAmount;

    public float BoughtMultiplier = 1f;
    //End//---------------------------------------------------------------------------------

    //Store Purchase Amounts//
    public float BuyEXPMultiplier = 30000f;

    public float BuyCOUNTMultiplier = 2500f;

    public float IncreaseAdRewardAmount = 100f;
    //End//---------------------------------------------------------------------------------

    ///Store Volume Data//
    public float MainVolume = 1f;

    public float MusicVolume = 1f;

    public float SfxandUIVolume = 1f;
    ///End//--------------------------------------------------------------------------------
}
