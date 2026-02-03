using UnityEngine;

public class CheatList : MonoBehaviour
{
   public MainButton SO;

   public TicketCount TC;

   public TicketShopPointMultipliers TSPM;

   public void GiveTickets()
   {
    TC.Ticketcount += 10000f;
   }

   public void GiveSoldiers()
   {
    SO.mainButtoncount += 1000000f;
   }

   public void GiveExp()
{
    SO.ExpCount += 1000f;
}

public void UpgradeRank()
{
    SO.ClicksTillNextLevel = 1;
}

public void AbilityActive()
{
    TSPM.ClicksTillActive = 1f;
}
}
