using UnityEngine;
using UnityEngine.UI;

public class FallingTicketClicked : MonoBehaviour
{
    public TicketCount TC;

    public GameObject Ticket;

    public Button ResetPosition;

    public Button ResetPositionifnotclicked;

    public Button PositionShake;

    public AudioSource GoodSound;

    private void Start()
    {
        Ticket.SetActive(false);
        PositionShake.onClick.Invoke();
    }

    private void Update()
    {
        if(TC.DropAtNumber >= 600f)
        {
            Ticket.SetActive(true);
            ResetPositionifnotclicked.onClick.Invoke();
        }
    }


    public void ClickedTicket()
    {
        GoodSound.Play();
        TC.Ticketcount += 1f;
        Ticket.SetActive(false);
        ResetPosition.onClick.Invoke();
    }
}
