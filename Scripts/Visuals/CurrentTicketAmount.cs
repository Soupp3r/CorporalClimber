using UnityEngine;
using TMPro;

public class CurrentTicketAmount : MonoBehaviour
{
    public TicketCount SO;
    public TMP_Text Text;

    // Update is called once per frame
    void Update()
    {
        Text.text = SO.Ticketcount.ToString();   
    }
}
