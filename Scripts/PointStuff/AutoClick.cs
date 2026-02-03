using UnityEngine;
using DamageNumbersPro;

public class AutoClick : MonoBehaviour
{
    ///Dependenices//----------------------------------------------------------------------
    [Header("Dependencies")]
    [Tooltip("Reference to shop, So i can grab the totalclickrate")]
    [SerializeField] private AutoClickerShop autoclickshop;
    [Tooltip("Reference to MainButton, SO i can grab the mainButtonCount; we will be adding to this")]
    [SerializeField] private MainButton SO;
    [Tooltip("Reference To the DNP for visuals")]
    [SerializeField] private AutoClickDamageNumberPopUp DNP;
    ///End//-------------------------------------------------------------------------------
    

    ///AutoClick//--------------------------------------------------------------------------
    public void Autoclick()
    {
        if(autoclickshop.TotalClickRate > 0f)
        {
            SO.mainButtoncount += autoclickshop.TotalClickRate;
            DNP.DNAppear();
        }
    }




}
