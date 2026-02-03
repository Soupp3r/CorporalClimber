using UnityEngine;

public class RewardTickets : MonoBehaviour
{
    public TicketCount TC;

    public void Purchasex15()
    {
        TC.Ticketcount += 15f;
    }

    public void Purchasex40()
    {
        TC.Ticketcount += 40f;
    }
}
