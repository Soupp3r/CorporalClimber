using UnityEngine;
using UnityEngine.UI;

public class ImageScaleSlider : MonoBehaviour
{
    [Header("Slider Reference")]
    public Slider scaleSlider;

    [Header("Image to Scale")]
    public Image targetImage;

    [Header("Scale Range")]
    public float minScale = 0.5f;
    public float maxScale = 2f;

    [Header("PlayerPrefs Settings")]
    public string playerPrefsKey = "ImageScale";
    public float defaultValue = 1f;

    void Start()
    {
        // Initialize the slider if not set in inspector
        if (scaleSlider == null)
            scaleSlider = GetComponent<Slider>();

        // Set up slider range
        if (scaleSlider != null)
        {
            scaleSlider.minValue = minScale;
            scaleSlider.maxValue = maxScale;
            
            // Load saved value or use default
            float savedScale = PlayerPrefs.GetFloat(playerPrefsKey, defaultValue);
            scaleSlider.value = savedScale;
            
            // Add listener for value changes
            scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);
            
            // Apply the scale immediately
            ApplyScale(savedScale);
        }
        else
        {
            Debug.LogError("Slider component not found!");
        }
    }

    private void OnSliderValueChanged(float newValue)
    {
        // Apply the new scale
        ApplyScale(newValue);
        
        // Save to PlayerPrefs
        PlayerPrefs.SetFloat(playerPrefsKey, newValue);
        PlayerPrefs.Save();
    }

    private void ApplyScale(float scaleValue)
    {
        if (targetImage != null)
        {
            targetImage.transform.localScale = new Vector3(scaleValue, scaleValue, 1f);
        }
    }

    // Public method to reset to default value
    public void ResetToDefault()
    {
        if (scaleSlider != null)
        {
            scaleSlider.value = defaultValue;
            PlayerPrefs.SetFloat(playerPrefsKey, defaultValue);
            PlayerPrefs.Save();
        }
    }

    // Public method to get current scale value
    public float GetCurrentScale()
    {
        return scaleSlider != null ? scaleSlider.value : defaultValue;
    }

    // Public method to set scale programmatically
    public void SetScale(float newScale)
    {
        if (scaleSlider != null)
        {
            newScale = Mathf.Clamp(newScale, minScale, maxScale);
            scaleSlider.value = newScale;
        }
    }

    void OnDestroy()
    {
        // Remove listener to prevent memory leaks
        if (scaleSlider != null)
        {
            scaleSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    // Optional: Context menu method for easy setup
    [ContextMenu("Auto-Setup Slider")]
    public void AutoSetupSlider()
    {
        if (scaleSlider == null)
            scaleSlider = GetComponent<Slider>();
        
        if (scaleSlider != null)
        {
            scaleSlider.minValue = minScale;
            scaleSlider.maxValue = maxScale;
            Debug.Log("Slider auto-setup completed");
        }
    }
}