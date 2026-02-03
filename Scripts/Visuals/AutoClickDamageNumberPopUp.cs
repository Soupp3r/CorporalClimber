using UnityEngine;
using DamageNumbersPro;

public class AutoClickDamageNumberPopUp : MonoBehaviour
{
    public AutoClick auto;
    public AutoClickerShop autoclickshop;
    public DamageNumber DN;
    public RectTransform UITarget;

    [Header("Offset Settings")]
    public Vector2 randomOffsetRange = new Vector2(200f, 200f);
    public Vector2 noSpawnZoneSize = new Vector2(100f, 100f); // Size in px of the "blocked" zone over the UITarget

    [Header("Scale Settings")]
    public Vector2 randomScaleRange = new Vector2(1.2f, 5f);
    public bool allowRandomRotation = true;

    [Tooltip("Enable this to override random scaling and use a fixed size")]
    public bool useFixedScale = false;

    [Tooltip("Only used if useFixedScale is enabled")]
    public float fixedScaleValue = 1f;

    [Header("Popup Settings")]
    public bool enablePopups = true; // New toggle to enable/disable popups

    private const string PlayerPrefsKey = "AutoClickDamagePopupsEnabled";

    void Start()
    {
        // Load the saved state on start
        LoadPopupState();
    }

    public void DNAppear()
    {
        // Check if popups are enabled
        if (!enablePopups)
            return;

        // 1. Get screen height-based max scale
        float screenHeight = Screen.height;
        float maxScale = Mathf.Max(screenHeight * 0.5f / 100f, randomScaleRange.y);

        // 2. Try to find a valid offset not overlapping the UITarget
        Vector2 offset;
        int attempts = 0;
        do
        {
            offset = new Vector2(
                Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
                Random.Range(-randomOffsetRange.y, randomOffsetRange.y)
            );
            attempts++;
        }
        while (
            Mathf.Abs(offset.x) < noSpawnZoneSize.x * 0.5f &&
            Mathf.Abs(offset.y) < noSpawnZoneSize.y * 0.5f &&
            attempts < 20
        );

        // 3. Calculate spawn position
        Vector2 spawnPosition = UITarget.anchoredPosition + offset;

        // 4. Spawn Damage Number if autoclick is not null
        DamageNumber dmg = null;
        if (auto != null)
        {
            string textToDisplay = "+" + autoclickshop.TotalClickRate.ToString("0.##");
            dmg = DN.SpawnGUI(UITarget.parent as RectTransform, spawnPosition, textToDisplay);
        }

        if (dmg == null) return;

        // 5. Set anchored position
        dmg.SetAnchoredPosition(UITarget.parent as RectTransform, UITarget, offset);

        // 6. Scale (fixed or random)
        float scale = useFixedScale ? fixedScaleValue : Random.Range(randomScaleRange.x, maxScale);
        dmg.transform.localScale = Vector3.one * scale;

        // 7. Random rotation
        if (allowRandomRotation)
        {
            float randomZ = Random.Range(-45f, 45f);
            dmg.transform.rotation = Quaternion.Euler(0f, 0f, randomZ);
        }

        Debug.Log($"Number Appears at offset {offset}, scale {scale}");
    }

    // Method to toggle popups on/off
    public void TogglePopups(bool enabled)
    {
        enablePopups = enabled;
        SavePopupState();
    }

    // Method to set popups state directly
    public void SetPopupsEnabled(bool enabled)
    {
        enablePopups = enabled;
        SavePopupState();
    }

    // Save the current state to PlayerPrefs
    private void SavePopupState()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, enablePopups ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Load the saved state from PlayerPrefs
    private void LoadPopupState()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            enablePopups = PlayerPrefs.GetInt(PlayerPrefsKey) == 1;
        }
        // If no key exists, keep the default value set in the inspector
    }

    // Optional: Method to reset to default (useful for debug)
    public void ResetPopupSetting()
    {
        PlayerPrefs.DeleteKey(PlayerPrefsKey);
        enablePopups = true; // Reset to default value
        PlayerPrefs.Save();
    }
}