using UnityEngine;
using DamageNumbersPro;

public class DamageNumberPopUp : MonoBehaviour
{
    public MainButton SO;
    public DamageNumber DN;
    public RectTransform UITarget;

    public Vector2 randomOffsetRange = new Vector2(200f, 200f);
    public Vector2 randomScaleRange = new Vector2(1.2f, 5f);
    public bool allowRandomRotation = true;
    public bool enablePopups = true; // New toggle to enable/disable popups

    public Vector2 noSpawnZoneSize = new Vector2(100f, 100f); // Size in px of the "blocked" zone over the UITarget

    private const string PlayerPrefsKey = "DamagePopupsEnabled";

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

        Vector2 spawnPosition = UITarget.anchoredPosition + offset;

        // 3. Spawn Damage Number
        string textToDisplay = "+" + SO.BoughtMultiplier.ToString("0.##");
        var dmg = DN.SpawnGUI(
            UITarget.parent as RectTransform,
            spawnPosition,
            textToDisplay
        );

        // 4. Set anchored position (so it animates well)
        dmg.SetAnchoredPosition(
            UITarget.parent as RectTransform,
            UITarget,
            offset
        );

        // 5. Random scale
        float scale = Random.Range(randomScaleRange.x, maxScale);
        dmg.transform.localScale = Vector3.one * scale;

        // 6. Random rotation
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