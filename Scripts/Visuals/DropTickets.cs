using UnityEngine;
using UnityEngine.UI; // Needed for the Button reference

public class DropTickets : MonoBehaviour
{
    public TicketCount SO;
   
    void Update()
    {
        if (SO.DropAtNumber >= 600f)
        {
            SO.Ticketcount += 1f * SO.MultiplyDrop;
            SO.DropAtNumber = 0f;
        }
    }

    public void ClickedTicket()
    {
        SO.DropAtNumber += 1f;
    }

}
