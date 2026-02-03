using UnityEngine;

[CreateAssetMenu(fileName = "TicketCount", menuName = "Scriptable Objects/TicketCount")]
public class TicketCount : ScriptableObject
{
    [Tooltip("Current amount of tickets player has")]
    public float Ticketcount;

    [Tooltip("Increase in Order to make the drop occur faster")]
    public float MultiplyDrop = 1f;

    [Tooltip("Increase to make the drop occur slower")]
    public float DropAtNumber;
}
