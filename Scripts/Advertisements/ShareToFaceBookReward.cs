/*  ────────────────────────────────────────────────────────────────
    ShareToFacebook.cs
    Put this on the “Share to Facebook” UI Button (or any GameObject)
    ──────────────────────────────────────────────────────────────── */
using UnityEngine;
using UnityEngine.Networking;   // for url-escaping

public class ShareToFacebookReward : MonoBehaviour
{
    [Header("Game Data")]
    public MainButton SO;                   // ScriptableObject that holds mainButtoncount

    [Header("Share Settings")]
    [Tooltip("Google-Play URL for Corporal Climber (you can paste it later).")]
    public string googlePlayURL = "";       // leave blank until you publish

    [Tooltip("One-time reward for sharing.")]
    public float rewardAmount = 200f;

    private const string SharedKey = "SharedToFacebook";

    // Call this from your UI Button’s OnClick
    public void ShareAndReward()
    {
        //----------------------------------------------
        // 1) Build the share text & open the FB dialog
        //----------------------------------------------
        string quote =
            $"I've earned {SO.mainButtoncount:F0} in Corporal Climber! " +
            $"Check out the free game on Google Play.";
        
        // Facebook share URL syntax:
        // https://www.facebook.com/sharer/sharer.php?u={link}&quote={text}
        string shareURL =
            "https://www.facebook.com/sharer/sharer.php?u=" +
            UnityWebRequest.EscapeURL(googlePlayURL) +
            "&quote=" +
            UnityWebRequest.EscapeURL(quote);

        Application.OpenURL(shareURL);

        //----------------------------------------------
        // 2) Reward only once
        //----------------------------------------------
        if (PlayerPrefs.GetInt(SharedKey, 0) == 0)
        {
            SO.mainButtoncount += rewardAmount;

            PlayerPrefs.SetInt(SharedKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"Player rewarded {rewardAmount} for sharing to Facebook.");
        }
        else
        {
            Debug.Log("Share reward already claimed; just opening Facebook share.");
        }
    }
}
