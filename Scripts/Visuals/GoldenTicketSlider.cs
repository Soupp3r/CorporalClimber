using UnityEngine;
using UnityEngine.UI;

public class GoldenTicketSlider : MonoBehaviour
{
    [Header("Slider")]
    [Tooltip("Slider used to track when the tickets will fall")]
    public Slider Ticketslider;

    [Header("Requirements")]
    [Tooltip("Required SO TicketSO")]
    public TicketCount TC;

    [Tooltip("Button that activates sound and effects")]
    public Button EffectPlayer;

    private bool triggered = false; // Prevents multiple triggers

    void Start()
    {
        // Set initial slider position to the value in the ScriptableObject
        Ticketslider.value = TC.DropAtNumber;

        // Listen for slider changes
        Ticketslider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float newValue)
    {
        TC.DropAtNumber = newValue;
        Debug.Log("float updated: " + TC.DropAtNumber);

        // If slider reaches max and hasn't triggered yet
        if (!triggered && Mathf.Approximately(newValue, Ticketslider.maxValue))
        {
            triggered = true; // Prevent multiple activations
            EffectPlayer.onClick.Invoke(); // Activate button's OnClick events
            Debug.Log("Max value reached! Button triggered.");
        }
    }

    void Update()
    {
        // Keep slider synced with external changes to DropAtNumber
        if (Ticketslider.value != TC.DropAtNumber)
        {
            Ticketslider.value = TC.DropAtNumber;
        }
    }
}
