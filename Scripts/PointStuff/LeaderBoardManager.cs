using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using TMPro;

public class LeaderBoardManager : MonoBehaviour
{
    [Tooltip("Reference to your MainButton script")]
    public MainButton mainButton;

    public string leaderboardID = "CgkIx6nGxNgKEAIQAQ";

    private bool isAuthenticated;
    private float mainButtonScore;
    private int retryCount;
    private const int maxRetries = 3;
    
    // Increased delay times for AAB builds
    private const float initialRetryDelay = 3f; // Increased from 2f
    private const float maxRetryDelay = 8f;     // Maximum delay for retries
    private const float leaderboardOpenDelay = 1.5f; // Increased delay before opening leaderboard

    [Tooltip("UI text to display status messages to the player")]
    public TextMeshProUGUI statusText;

    void Start()
    {
        // Initialize the Play Games Platform
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        
        SignInSilently();
    }

    void Update()
    {
        if (mainButton != null)
            mainButtonScore = mainButton.mainButtoncount;
    }

    private bool HasInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private void ShowStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null)
            statusText.text = message;
    }

    private void SignInSilently()
    {
        if (!HasInternetConnection())
        {
            ShowStatus("No internet connection. Cannot sign in.");
            return;
        }

        ShowStatus("Attempting silent sign-in...");
        PlayGamesPlatform.Instance.Authenticate((status) =>
        {
            isAuthenticated = (status == SignInStatus.Success);
            ShowStatus(isAuthenticated ? "Silent sign-in success." : "Silent sign-in failed. Status: " + status);
        });
    }

    private void PromptSignIn(System.Action onSuccess = null)
    {
        if (!HasInternetConnection())
        {
            ShowStatus("No internet connection. Cannot sign in.");
            return;
        }

        ShowStatus("Prompting sign-in...");
        PlayGamesPlatform.Instance.ManuallyAuthenticate((status) =>
        {
            isAuthenticated = (status == SignInStatus.Success);
            if (isAuthenticated)
            {
                ShowStatus("Prompt sign-in success.");
                retryCount = 0;
                onSuccess?.Invoke();
            }
            else
            {
                ShowStatus("Prompt sign-in failed. Status: " + status);
                RetrySignIn(onSuccess);
            }
        });
    }

    private void RetrySignIn(System.Action onSuccess)
    {
        if (retryCount < maxRetries)
        {
            retryCount++;
            
            // Calculate increasing delay with each retry (exponential backoff)
            float retryDelay = Mathf.Min(initialRetryDelay * Mathf.Pow(1.5f, retryCount), maxRetryDelay);
            
            ShowStatus($"Retrying sign-in attempt {retryCount}/{maxRetries} in {retryDelay:F1}s...");
            StartCoroutine(RetryAfterDelay(retryDelay, onSuccess));
        }
        else
        {
            ShowStatus("Max sign-in retries reached. Aborting.");
        }
    }

    private IEnumerator RetryAfterDelay(float delay, System.Action onSuccess)
    {
        yield return new WaitForSeconds(delay);
        PromptSignIn(onSuccess);
    }

    public void OnLeaderboardButtonPressed()
    {
        ShowStatus("Leaderboard button pressed.");
        if (!isAuthenticated)
        {
            PromptSignIn(() => ReportScoreAndShow());
        }
        else
        {
            ReportScoreAndShow();
        }
    }

    private void ReportScoreAndShow()
    {
        if (!HasInternetConnection())
        {
            ShowStatus("No internet connection. Cannot report score.");
            return;
        }

        long scoreToReport = (long)mainButtonScore;
        ShowStatus($"Reporting score {scoreToReport} to leaderboard {leaderboardID}...");

        PlayGamesPlatform.Instance.ReportScore(scoreToReport, leaderboardID, (bool success) =>
        {
            ShowStatus(success ? "Score reported successfully." : "Failed to report score.");
            StartCoroutine(OpenLeaderboardDelayed());
        });
    }

    private IEnumerator OpenLeaderboardDelayed()
    {
        // Wait longer before opening the leaderboard for AAB builds
        yield return new WaitForSeconds(leaderboardOpenDelay);
        
        ShowStatus("Opening leaderboard UI...");
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
        
        // Additional wait to ensure leaderboard has time to load
        yield return new WaitForSeconds(1f);
        
        // Check if we need to handle any post-load operations
    }
}