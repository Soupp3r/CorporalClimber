using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ExpSlider : MonoBehaviour
{
    [Header("Config")]
    public MainButton mainButtonSO; // Drag your MainButton SO here
    public float smoothFillSpeed = 5f; // Set to 0 for instant updates

    private Slider _slider;
    private int _lastClicks;
    private int _maxClicks;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _lastClicks = mainButtonSO.ClicksTillNextLevel;
        CalculateMaxClicks();
        ResetSlider();
    }

    void Update()
    {
        // Only update if clicks changed (no wasteful updates)
        if (mainButtonSO.ClicksTillNextLevel != _lastClicks)
        {
            _lastClicks = mainButtonSO.ClicksTillNextLevel;
            
            float progress = 1f - ((float)_lastClicks / _maxClicks);
            
            if (smoothFillSpeed > 0)
                _slider.value = Mathf.Lerp(_slider.value, progress, smoothFillSpeed * Time.deltaTime);
            else
                _slider.value = progress;

            // Auto-reset when reaching upgrade
            if (_lastClicks <= 0)
            {
                CalculateMaxClicks();
                _slider.value = 0;
            }
        }
    }

    void CalculateMaxClicks()
    {
        // Matches your PointCollecter's formula exactly
        _maxClicks = Mathf.CeilToInt(
            (100 * mainButtonSO.ExpMultiplier) / 
            (mainButtonSO.BoughtMultiplier * mainButtonSO.BonusAmount)
        );
    }

    void ResetSlider()
    {
        _slider.value = 0;
    }
}