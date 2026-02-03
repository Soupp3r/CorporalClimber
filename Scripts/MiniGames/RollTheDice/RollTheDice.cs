using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class RollTheDice : MonoBehaviour
{
    [Header("🎲 Dice UI Elements")]
    public Image diceImage1;
    public Image diceImage2;
    public List<Sprite> diceSprites;

    [Header("📝 Text Components")]
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI wagerText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerRollText;
    public TextMeshProUGUI botRollText;

    [Header("👤 Player Profile")]
    public Image playerImage;
    public Sprite defaultProfileImage;

    [Header("🎛️ Buttons")]
    public Button rollButton;
    public Button stayButton;
    public Button upArrowButton;
    public Button downArrowButton;
    public Button effectsButton;

    [Header("📦 Scriptable Objects")]
    public MainButton MainButton;
    public TicketCount TicketCount;

    [Header("🎵 Sounds")]
    public AudioSource audioSource;
    public AudioClip goodSound;
    public AudioClip badSound;
    public AudioClip textDisplaySound;

    [Header("⚙️ Gameplay Settings")]
    public int wagerIncrement = 10000;
    public float rollDelay = 1.5f;
    public float botRollDelay = 3f;
    [Range(0, 100)] public int houseWinPercentage = 80;

    [Header("🚫 Disable During Game")]
    public List<GameObject> objectsToDisableDuringGame;

    private int currentWager = 0;
    private int playerTotal = 0;
    private int botTotal = 0;
    private bool gameActive = false;
    private bool hasRolled = false;

    private List<string> bustTexts = new List<string>()
    {
        "You busted! Game over!",
        "You busted! Better luck next time!",
        "You busted! The house wins!",
        "You busted! Try again!",
        "You busted! Too bad!"
    };

    private List<string> matchTexts = new List<string>()
    {
    "We matched! Roll again!",
    "It's a tie! Let's go another round!",
    "Equal rolls! The battle continues!",
    "Match! House always wins this round!",
    "Tie game! Try your luck again!",
    "Same score! The dice favor neither!",
    "Dead heat! Roll those dice once more!",
    "Perfect match! The tension builds!"
    };

    private List<string> winTexts = new List<string>()
    {
        "You won! Congratulations!",
        "You won! Great job!",
        "You won! Victory is yours!",
        "You won! The house loses!",
        "You won! Amazing rolls!"
    };

    private List<string> loseTexts = new List<string>()
    {
        "You lose! House wins!",
        "You lose! Better luck next time!",
        "You lose! Try again!",
        "You lose! The house was better!",
        "You lose! Too bad!"
    };

    private List<string> houseBustTexts = new List<string>()
    {
        "House busted! You won!",
        "House busted! Victory is yours!",
        "House busted! Congratulations!",
        "House busted! You got lucky!",
        "House busted! You win!"
    };

    private List<string> endGameTexts = new List<string>()
    {
        "Game over! play again?",
        "Try again if you dare!",
        "Care to play again?",
        "Game reset."
    };

    void Update()
    {

        if(MainButton.mainButtoncount <= 0f)
        {
            MainButton.mainButtoncount = 0f;
        }
    }

    void Start()
    {
        // Initialize text lists if null
        bustTexts = bustTexts ?? new List<string>();
        winTexts = winTexts ?? new List<string>();
        loseTexts = loseTexts ?? new List<string>();
        houseBustTexts = houseBustTexts ?? new List<string>();
        endGameTexts = endGameTexts ?? new List<string>();

        rollButton.onClick.AddListener(OnRollClicked);
        stayButton.onClick.AddListener(OnStayClicked);
        upArrowButton.onClick.AddListener(OnUpArrowClicked);
        downArrowButton.onClick.AddListener(OnDownArrowClicked);

#if UNITY_EDITOR
        playerNameText.text = "Player (Editor)";
        playerImage.sprite = defaultProfileImage;
#else
        AuthenticateGooglePlay();
#endif
        UpdateUI();
        SetGameObjectsActive(true);
    }

    void AuthenticateGooglePlay()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate((SignInStatus status) =>
        {
            if (status == SignInStatus.Success)
            {
                string userName = PlayGamesPlatform.Instance.localUser.userName;
                playerNameText.text = userName;

                Texture2D profilePic = PlayGamesPlatform.Instance.localUser.image;
                if (profilePic != null)
                {
                    StartCoroutine(LoadProfileImage(profilePic));
                }
                else
                {
                    playerImage.sprite = defaultProfileImage;
                }
            }
            else
            {
                playerNameText.text = "Player";
                playerImage.sprite = defaultProfileImage;
            }
        });
    }

    IEnumerator LoadProfileImage(Texture2D image)
    {
        if (image != null)
        {
            Sprite profileSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.one * 0.5f);
            playerImage.sprite = profileSprite;
        }
        yield return null;
    }

    void OnRollClicked()
    {
        if (gameActive) return;

        if (currentWager == 0)
        {
            ShowText("You must place a wager first!", true);
            return;
        }

        
        if (TicketCount.Ticketcount < 1)
        {
            ShowText("Not enough tickets!", true);
            return;
        }

        PlaySound(goodSound);
        hasRolled = true;
        StartCoroutine(PlayerRollRoutine());
    }

    void OnStayClicked()
    {
        if (gameActive || !hasRolled || playerTotal == 0) return;

        PlaySound(goodSound);
        StartCoroutine(BotTurnRoutine());
    }

    void OnUpArrowClicked()
    {
        if (gameActive) return;

         if (TicketCount.Ticketcount < 1)
    {
        ShowText("Not enough tickets to play!", true);
        return;
    }

        if (MainButton.mainButtoncount >= wagerIncrement)
        {
            PlaySound(goodSound);
            currentWager += wagerIncrement;
            wagerText.text = currentWager.ToString();
        }
        else
        {
            ShowText("Not enough soldiers to wager!", true);
        }
    }

    void OnDownArrowClicked()
    {
        if (gameActive) return;

        if (currentWager == 0)
        {
            PlaySound(badSound);
            return;
        }
        PlaySound(goodSound);
        currentWager -= wagerIncrement;
        if (currentWager < 0) currentWager = 0;
        wagerText.text = currentWager.ToString();
    }

    IEnumerator PlayerRollRoutine()
    {
        SetGameActive(false);
        SetGameObjectsActive(false);
        
        // Roll the dice immediately
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        playerTotal += dice1 + dice2;

        // Show the actual dice immediately
        diceImage1.sprite = diceSprites[dice1 - 1];
        diceImage2.sprite = diceSprites[dice2 - 1];

        ShowText($"You rolled {dice1} and {dice2}!", false);
        playerRollText.text = playerTotal.ToString();

        // Just wait for visual effect
        yield return new WaitForSeconds(rollDelay);

        if (playerTotal > 21)
        {
            ShowText(GetRandomText(bustTexts), true);
            LoseWager();
        }
        else
        {
            SetGameActive(true);
        }
    }

    IEnumerator BotTurnRoutine()
    {
        SetGameActive(false);
        SetGameObjectsActive(false);
        yield return new WaitForSeconds(botRollDelay);
        
        while (botTotal < playerTotal && botTotal <= 21)
        {
            effectsButton.onClick.Invoke();
            
            int dice1, dice2;
            
            // Rigged roll based on houseWinPercentage
            if (Random.Range(0, 100) < houseWinPercentage && playerTotal <= 21)
            {
                // House gets a perfect roll to win or match
                int needed = playerTotal - botTotal;
                if (needed <= 12) // Possible with two dice
                {
                    dice1 = Mathf.Clamp(needed / 2, 1, 6);
                    dice2 = needed - dice1;
                    if (dice2 > 6) 
                    {
                        dice2 = 6;
                        dice1 = needed - 6;
                    }
                }
                else
                {
                    // Just get highest possible without busting
                    dice1 = 6;
                    dice2 = 6;
                }
            }
            else
            {
                // Regular random roll
                dice1 = Random.Range(1, 7);
                dice2 = Random.Range(1, 7);
            }
            
            botTotal += dice1 + dice2;

            // Show bot's dice immediately
            diceImage1.sprite = diceSprites[dice1 - 1];
            diceImage2.sprite = diceSprites[dice2 - 1];

            ShowText($"House rolled {dice1} and {dice2}!", false);
            botRollText.text = botTotal.ToString();

            yield return new WaitForSeconds(botRollDelay);

            if (botTotal > 21) break;
        }

        // Check for match first
        if (botTotal == playerTotal)
{
    ShowText(GetRandomText(matchTexts), true); // Use random match text
    MatchPush();
}
        else if (botTotal > 21)
        {
            ShowText(GetRandomText(houseBustTexts), false);
            WinWager();
        }
        else
        {
            CompareResults();
        }
    }

    void MatchPush()
{
    // Nothing happens - player keeps their wager and ticket
    // No changes to MainButton.mainButtoncount or TicketCount.Ticketcount
    
    // Just save the current values (no changes)
    PlayerPrefs.SetFloat("mainCount", MainButton.mainButtoncount);
    PlayerPrefs.SetFloat("TicketAmount", TicketCount.Ticketcount);
    PlayerPrefs.Save();
    
    ResetGame();
}

    void CompareResults()
    {
        if (botTotal == playerTotal)
    {
        ShowText(GetRandomText(matchTexts), true); // Use random match text
        MatchPush();
    }
        else if (botTotal > playerTotal)
        {
            ShowText(GetRandomText(loseTexts), true);
            LoseWager();
        }
        else if (botTotal > 21)
    {
        ShowText(GetRandomText(houseBustTexts), false);
        WinWager();
    }
        else
        {
            ShowText(GetRandomText(winTexts), false);
            WinWager();
        }
    }

    // Helper method to safely get random text
    private string GetRandomText(List<string> texts)
    {
        if (texts == null || texts.Count == 0)
        {
            // Fallback messages
            if (texts == bustTexts) return "You busted!";
            if (texts == winTexts) return "You won!";
            if (texts == loseTexts) return "You lost!";
            if (texts == houseBustTexts) return "House busted! You win!";
            return "Game over!";
        }
        return texts[Random.Range(0, texts.Count)];
    }

    void LoseWager()
    {
 if (TicketCount.Ticketcount > 0)
    {
        TicketCount.Ticketcount -= 1;
    }

        MainButton.mainButtoncount -= currentWager;
        
        if (TicketCount.Ticketcount < 0) TicketCount.Ticketcount = 0;
    if (MainButton.mainButtoncount < 0) MainButton.mainButtoncount = 0;

        // Save values
        PlayerPrefs.SetFloat("mainCount", MainButton.mainButtoncount);
        PlayerPrefs.SetFloat("TicketAmount", TicketCount.Ticketcount);
        PlayerPrefs.Save();
        
        ResetGame();
    }

    void WinWager()
    {
        int ticketReward = 1;
        MainButton.mainButtoncount += currentWager * 2;
        TicketCount.Ticketcount += ticketReward;
        
        // Save values
        PlayerPrefs.SetFloat("mainCount", MainButton.mainButtoncount);
        PlayerPrefs.SetFloat("TicketAmount", TicketCount.Ticketcount);
        PlayerPrefs.Save();
        
        ResetGame();
    }

    void ResetGame()
    {
        ShowText(GetRandomText(endGameTexts), false);
        currentWager = 0;
        playerTotal = 0;
        botTotal = 0;
        hasRolled = false;
        playerRollText.text = "0";
        botRollText.text = "0";
        wagerText.text = "0";
        SetGameActive(true);
        SetGameObjectsActive(true);
    }

    void SetGameActive(bool active)
    {
        gameActive = !active;
        rollButton.interactable = active;
        stayButton.interactable = active && hasRolled && playerTotal > 0 && playerTotal <= 21;
        upArrowButton.interactable = active;
        downArrowButton.interactable = active;
    }

    void SetGameObjectsActive(bool active)
    {
        foreach (GameObject obj in objectsToDisableDuringGame)
        {
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }
    }

    void UpdateUI()
    {
        playerRollText.text = playerTotal.ToString();
        botRollText.text = botTotal.ToString();
        wagerText.text = currentWager.ToString();
    }

    void ShowText(string message, bool isBad)
    {
        displayText.text = message;
        PlaySound(textDisplaySound);
        if (isBad) PlaySound(badSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}