using UnityEngine;

public class RewardForRating : MonoBehaviour
{
    [Header("Required References")]
    public MainButton SO;
    public PointCollecter pointcollecter;

    [Header("Rating Settings")]
    [Tooltip("Leave this blank for now and fill it in later with your Play Store link.")]
    public string googlePlayURL;

    [Tooltip("One-time reward for rating the game.")]
    public float ratingReward = 200f;

    // Use a more unique key to avoid conflicts
    private const string HasRatedKey = "RatingReward_Claimed";

    void Start()
    {
        // Debug to check if reward was already claimed
        bool alreadyRewarded = PlayerPrefs.GetInt(HasRatedKey, 0) == 1;
        Debug.Log($"Rating reward already claimed: {alreadyRewarded}");
    }

    public void OpenRatingPage()
    {
        Debug.Log($"BEFORE RATING - Main: {SO.mainButtoncount}, Multiplier: {SO.BoughtMultiplier}");
        Debug.Log("OpenRatingPage called");

        // Always open the Play Store page
        if (!string.IsNullOrEmpty(googlePlayURL))
        {
            Application.OpenURL(googlePlayURL);
            Debug.Log($"Opening URL: {googlePlayURL}");
        }
        else
        {
            Debug.Log("Google Play URL not set, but OpenRatingPage Test works");
        }

        // Only give reward once
        if (PlayerPrefs.GetInt(HasRatedKey, 0) == 0)
        {
            Debug.Log($"Giving rating reward: {ratingReward}");

            // Store original values for debugging
            float originalMainCount = SO.mainButtoncount;

            // Give reward
            SO.mainButtoncount += ratingReward;

            PlayerPrefs.SetInt(HasRatedKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"Main count: {originalMainCount} -> {SO.mainButtoncount}");
            Debug.Log($"Rating reward of {ratingReward} given successfully.");
        }
        else
        {
            Debug.Log("Rating reward already claimed. Only opening store page.");
        }
        Debug.Log($"AFTER RATING - Main: {SO.mainButtoncount}, Multiplier: {SO.BoughtMultiplier}");
    }
    
    // Add a method to reset the reward (for testing)
    public void ResetRatingReward()
    {
        PlayerPrefs.DeleteKey(HasRatedKey);
        PlayerPrefs.Save();
        Debug.Log("Rating reward reset. Player can claim again.");
    }
}