using UnityEngine;
using UnityEngine.UI;

public class DroppingTickets : MonoBehaviour
{

    public TicketCount SO;

    [Header("Buttons")]
    [Tooltip("Drop is the button to click when it is time for the tickets to fall")]
    public Button Drop;

    public void DropTickets()
    {
        if(SO.DropAtNumber >= 600f)
        {
            Drop.onClick.Invoke();
            SO.DropAtNumber = 0f;
        }
    }

    public void TicketClick()
    {
        SO.Ticketcount += 1f;
    }

    public void AddToDropAmount()
    {
        SO.DropAtNumber += 1f * SO.MultiplyDrop;
    }

}
