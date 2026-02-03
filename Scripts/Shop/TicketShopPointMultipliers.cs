using UnityEngine;
using TMPro;
using UnityEngine.UI;

/////////////////////////////////////////////////////////////////////////////////////////
/// /////////////////    This Script is work as the Ticket Shop Multipliers and//////////
/// ////////////////     The function of the Multipliers ////////////////////////////////
/// /////////////////////////////////////////////////////////////////////////////////////

public class TicketShopPointMultipliers : MonoBehaviour
{
    /// This Script should
    /// -------------------------------------------------------------------------------
    /// 1. Have a voids for purchasing
    /// 
    /// 2. Each purchase void will add to an int and turn off the button
    /// 
    /// 3. The player should have to purchase them in order, so it will also,
    ///    turn on the next button
    /// 
    /// 4. it should also check if the player has enough to purchase, if not
    ///    play an error sound, if they do - the tickets and play a good sound
    /// 
    /// 
    /// 6. Each functionality should after clicking a certain amount of times,
    ///    activate the purchased ability, and when they stop clicking, restart
    ///    the amount of tmes to click till activate
    /// 
    /// 7. We need to also update the TMP_Text
    /// 
    /// 8. After each purchase activate the feeback, using buttons
    /// 
    /// 
    /// ---------------------------------------------------------------------------------
    /// /////////////////////////////////////////////////////////////////////////////////////
    ///                      REQUIREMENTS                                                 ///
    /// /////////////////////////////////////////////////////////////////////////////////////
    /// 1. 3 purcashing voids
    /// 2. purchase floats, clicks till active float, 3 pricingfloats
    /// 3. 2 audiousources, 1 good , 1 bad
    /// 4. 2 buttons , 1 Purchase feedback button, 1 Ability active button
    /// 5. 1 TMP_Text to update
    /// 6. ScriptableObject in order to purchase and multiply
    /// ///////////////////////////////////////////////////////////////////////////////////////
    
    ////Inspector/////////////////////////////////////////////////////
    [Header("Floats")]
    [Tooltip("float to control the puchase float, the controls what displays")]
    public float Purchasefloat = 0f;
    [Tooltip("This float controls the mulitplied amount")]
    public float MultiplyByFloat = 0f;
    [Tooltip("float to contol the amount of clicks needed to start the ability")]
    public float ClicksTillActive = 600f;
    [Tooltip("Price for Purchasing")]
    public float PurchasePrice = 15f;
    ///---------------------------------------------------------------------------------
    ///
    [Header("AudioSource's")]
    [Tooltip("Sound to play for a good feedback")]
    public AudioSource GoodSound;
    [Tooltip("Sound to play for a bad feedback")]
    public AudioSource BadSound;

    ///----------------------------------------------------------------------------------
    /// 
    [Header("Feedback Buttons")]
    [Tooltip("Button to play for purchase")]
    public Button PurchaseFeedback;
    [Tooltip("Button to play for Ability to active")]
    public Button AbilityFeedback;
    ///------------------------------------------------------------------------------------
    /// 
    [Header("UI Text Components")]
    [Tooltip("The Text Component to update when the player purchases the ability's")]
    public TMP_Text MultiplierText;

    [Header("ScriptableObjects")]
    [Tooltip("This is the ScriptableObject that holds the Tickets, Targeting Ticketcount on this")]
    public TicketCount TC;///Targeting Ticketcount on this
    [Tooltip("This is the ScriptableObject that hold the points to multiply, Targeting BoughtMultiplier on this")]
    public MainButton SO;///Targeting BoughtMultiplier on this


    private bool multiplieractive;

    [Header("GameObject, PurchaseButtons to be turnedOff")]
    [Tooltip("x2 Button in TicketShop")]
    public GameObject x2ActiveMultiplierButton;
    [Tooltip("x3 Button in TicketShop")]
    public GameObject x3ActiveMultiplierButton;
    [Tooltip("x5 Button in TicketShop")]
    public GameObject x5ActiveMultiplierButton;
    [Tooltip("CheckMark to activate after all is bought")]
    public GameObject CheckMark;
    


    /// Update function//
    /// 
    private void Update()
    {
        ///Update Text to needed text///
        MultiplierText.text = "x" + MultiplyByFloat.ToString();
        ///-----------------------------------------------------------------
        /// 
        /// This is to add the functionality when it is active
        
        Functionality();
        ///--------------------------------------------------------------------
        if(Purchasefloat == 1f)
        {
            x2ActiveMultiplierButton.SetActive(false);
        }else if(Purchasefloat == 2f)
        {
            x3ActiveMultiplierButton.SetActive(false);
            x2ActiveMultiplierButton.SetActive(false);
        }else if(Purchasefloat == 3f)
        {
            x3ActiveMultiplierButton.SetActive(false);
            x2ActiveMultiplierButton.SetActive(false);
            x5ActiveMultiplierButton.SetActive(false);
            CheckMark.SetActive(true);

        }else if(Purchasefloat >= 4f)
        {
            x3ActiveMultiplierButton.SetActive(false);
            x2ActiveMultiplierButton.SetActive(false);
            x5ActiveMultiplierButton.SetActive(false);
            Purchasefloat = 3f;
        }

    }

    ////Purchase Void////
    /// 
    public void PurchaseAbility()///Assign to purchase Buttons
    {
        ///First Purchase//--------------------------------------------------
        if(TC.Ticketcount >= PurchasePrice && Purchasefloat == 0f)
        {
            TC.Ticketcount -= PurchasePrice;
            MultiplyByFloat += 2f;
            Purchasefloat += 1f;
            PurchasePrice += 15f;
            GoodSound.Play();
            PurchaseFeedback.onClick.Invoke();
        }
        ///SecoundPurchase//----------------------------------------------------
        else if(TC.Ticketcount >= PurchasePrice && Purchasefloat == 1f)
        {
            TC.Ticketcount -= PurchasePrice;
            MultiplyByFloat += 1f;
            Purchasefloat += 1f;
            PurchasePrice += 15f;
            GoodSound.Play();
            PurchaseFeedback.onClick.Invoke();
        }
        ///ThirdPurchase//--------------------------------------------------------------
        else if(TC.Ticketcount >= PurchasePrice && Purchasefloat == 2f)
        {
            TC.Ticketcount -= PurchasePrice;
            MultiplyByFloat += 2f;
            Purchasefloat += 1f;
            GoodSound.Play();
            PurchaseFeedback.onClick.Invoke();
        }else
        {
            BadSound.Play();
        }
    }

    ///Functionality////
    /// 
   ///Functionality////
public void Functionality()
{
    if(ClicksTillActive <= 0f && Purchasefloat >= 1f && multiplieractive)
    {
        ///--------------------------Visual Functions and functions-----------------------------
        AbilityFeedback.onClick.Invoke();
        ///This will activate a feedback, that will fade in the image
        /// this image holds the TMP_Text
        /// it will begin a timer to reset the clickstillactive
        ///-----------------------------------------------------------------------------

        ///--------------------- Number to multiply by -----------------------------------
        SO.BoughtMultiplier *= MultiplyByFloat;
        /// This will multiply clicks by the multiply by float
        ///-------------------------------------------------------------------------------

        ///---------------------- Boolen flag to prevent constant multiplying-----------
        multiplieractive = false;
        /// This will deactivate the multiplier //--------------------------------
    }
    else if(ClicksTillActive <= 0f && !multiplieractive)
    {
        // Do nothing — already activated
    }
    else if(ClicksTillActive > 0f)
    {
        multiplieractive = true;
    }
}


    ///Call this function for when clicking the clicker button////
    /// 
    public void SubtractClicksTillActive()
    {
       if(Purchasefloat >= 1f) 
        {
            ClicksTillActive -= 1f;
        }
    }
    public void ResetClicktillActive()
    {
        ClicksTillActive = 600f;
        SO.BoughtMultiplier /= MultiplyByFloat;
        multiplieractive = false;
    }

}
