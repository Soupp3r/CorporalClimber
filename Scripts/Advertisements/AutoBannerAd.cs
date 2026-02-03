using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoogleMobileAds;
using GoogleMobileAds.Api;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class AutoBannerAd : MonoBehaviour
{
    [Header("AdMob Settings")]
    public AdMobSettings adMobSettings;

    [Header("Banner Position")]
    public AdPosition position = AdPosition.Bottom;

    [Header("Placeholder Visuals")]
    public Color testColor = new Color(0.2f, 0.6f, 1f, 0.7f);
    public Color productionColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public string placeholderText = "Banner Ad";

    private BannerView bannerView;
    private Image image;
    private TextMeshProUGUI tmpText;
    private RectTransform rectTransform;
    private bool isAdInitialized = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SetupPlaceholder();
        UpdateAnchors();
    }

    void Start()
    {
        UpdateVisuals();

        // Initialize Mobile Ads
        MobileAds.Initialize(initStatus =>
        {
            isAdInitialized = true;
            Debug.Log("AdMob initialized successfully", this);
            RequestBanner();
        });
    }

    void OnEnable()
    {
        // Re-request banner when object becomes active
        if (isAdInitialized && bannerView == null)
        {
            RequestBanner();
        }
    }

    void OnDisable()
    {
        // Hide banner when object is disabled
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    private void SetupPlaceholder()
    {
        image = GetComponent<Image>();
        image.type = Image.Type.Sliced;

        tmpText = GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText == null)
        {
            GameObject textObj = new GameObject("PlaceholderText (TMP)");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = Vector3.zero;

            tmpText = textObj.AddComponent<TextMeshProUGUI>();
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.white;
            tmpText.text = placeholderText;
            tmpText.enableAutoSizing = true;
            tmpText.fontSizeMin = 8;
            tmpText.fontSizeMax = 24;
            tmpText.margin = new Vector4(5, 5, 5, 5);

            RectTransform textRt = textObj.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.sizeDelta = Vector2.zero;
            textRt.anchoredPosition = Vector2.zero;
            textRt.localScale = Vector3.one;
        }

        if (tmpText != null)
        {
            tmpText.enableAutoSizing = true;
            tmpText.fontSizeMin = 8;
            tmpText.fontSizeMax = 24;
        }
    }

    private void UpdateAnchors()
    {
        if (rectTransform == null) return;

        switch (position)
        {
            case AdPosition.Top:
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case AdPosition.Bottom:
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                rectTransform.pivot = new Vector2(0.5f, 0f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
        }
    }

    private void RequestBanner()
    {
        if (adMobSettings == null)
        {
            Debug.LogError("AdMobSettings reference is missing!", this);
            return;
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        string adUnitId = adMobSettings.BannerID;
        Debug.Log($"Requesting banner ad: {adUnitId}", this);

        // Use adaptive banner size
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(adUnitId, adSize, position);

        // Register event handlers
        bannerView.OnBannerAdLoaded += HandleBannerAdLoaded;
        bannerView.OnBannerAdLoadFailed += HandleBannerAdLoadFailed;
        bannerView.OnAdPaid += HandleAdPaid;

        // ✅ FIX: build AdRequest properly
        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    private void HandleBannerAdLoaded()
    {
        Debug.Log("Banner ad loaded successfully", this);

        image.enabled = false;
        if (tmpText != null) tmpText.enabled = false;

        bannerView.Show();
    }

    private void HandleBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError($"Banner failed to load: {error.GetMessage()}", this);

        UpdateVisuals();
        Invoke(nameof(RequestBanner), 30f);
    }

    private void HandleAdPaid(AdValue adValue)
    {
        Debug.Log($"Banner ad paid: {adValue.Value} {adValue.CurrencyCode}");
    }

    private void UpdateVisuals()
    {
        if (adMobSettings == null) return;

        image.enabled = true;
        if (tmpText != null)
        {
            tmpText.enabled = true;
            tmpText.text = $"{placeholderText}\n({adMobSettings.currentEnvironment} Mode)";
        }

        image.color = adMobSettings.currentEnvironment == AdMobSettings.Environment.Test
            ? testColor
            : productionColor;
    }

    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        CancelInvoke(nameof(RequestBanner));
    }

    public void RefreshBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
        RequestBanner();
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
            image.enabled = false;
            if (tmpText != null) tmpText.enabled = false;
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            image.enabled = true;
            if (tmpText != null) tmpText.enabled = true;
        }
    }
}
