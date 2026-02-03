using UnityEngine;
using TMPro;

public class DisplaySoldierandTicketAmount : MonoBehaviour
{
    [Header("Texts UI")]
    public TextMeshProUGUI soldiersText;
    public TextMeshProUGUI TicketText;

    [Header("Requirements")]
    public MainButton SO;
    public TicketCount TC;

    private void Update()
    {
        soldiersText.text = Mathf.Floor(SO.mainButtoncount).ToString("F0");
        TicketText.text = Mathf.Floor(TC.Ticketcount).ToString("F0");
    }
}
