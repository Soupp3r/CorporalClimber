using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankUpgradeMenu : MonoBehaviour
{
    [Header("Rank UI Button")]
    [Tooltip("Rank Button that activates MMF_Player inside RankUpgrade")]
    public Button RankUpgrade;

    [Header("SO Requirements")]
    [Tooltip("SO that holds Clickstillnextlevel")]
    public MainButton SO;
    [Tooltip("SO that holds the TicketCount of the player")]
    public TicketCount TC;

    [Header("Audio Requirements")]
    [Tooltip("Bad Sound")]
    public AudioSource BadSound;
    [Tooltip("Good Sound")]
    public AudioSource GoodSound;

    [Header("Pricing")]
    [Tooltip("Ticket Pricing also know as Ticketcount")]
    public float TicketPrice = 10f;
    [Tooltip("SoldiersPrice, also know as mainButtoncount")]
    public float SoldierPrice = 10000f;

    [Header("Text Requirements")]
    [Tooltip("TicketPrice Text")]
    public TextMeshProUGUI TicketPriceText;
    [Tooltip("SoldierPrice Text")]
    public TextMeshProUGUI SoldierPriceText;

    [Header("Scaling Settings")]
    public float soldierBasePrice = 10000f;
    public float ticketBasePrice = 10f;
    public int currentRank = 1;
    public float soldierExponent = 1.08f; // Reduced from ~1.2 to 1.08 for better scaling
    public float ticketExponent = 1.02f;  // Reduced from ~1.2 to 1.05 for better scaling

    private void Start()
    {
        // Load current rank from player prefs if you want to persist it
        // currentRank = PlayerPrefs.GetInt("CurrentRank", 1);
        UpdatePrices();
    }

    private void Update()
    {
        TicketPriceText.text = Mathf.RoundToInt(TicketPrice).ToString();
        SoldierPriceText.text = Mathf.RoundToInt(SoldierPrice).ToString();
    }
    
    private void UseRankButton()
    {
        RankUpgrade.onClick.Invoke();
    }

    public void UpgradeRank()
    {
        if(SO.mainButtoncount >= SoldierPrice && TC.Ticketcount >= TicketPrice)
        {
            GoodSound.Play();
            SO.mainButtoncount -= SoldierPrice;
            TC.Ticketcount -= TicketPrice;
            UseRankButton();
            ClicksTillNextLevelZero();
            
            // Increase rank and update prices for next upgrade
            currentRank++;
            UpdatePrices();
            
            // Save progress if needed
            // PlayerPrefs.SetInt("CurrentRank", currentRank);
            // PlayerPrefs.Save();
        }
        else
        {
            BadSound.Play();
        }
    }

    private void UpdatePrices()
    {
        // Use exponential scaling with a more reasonable exponent
        SoldierPrice = soldierBasePrice * Mathf.Pow(soldierExponent, currentRank - 1);
        TicketPrice = ticketBasePrice * Mathf.Pow(ticketExponent, currentRank - 1);
    }

    private void ClicksTillNextLevelZero()
    {
        SO.ClicksTillNextLevel = 0;
    }
}