using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ToggleEffect
{
    public GameObject parentObject; // Drag the parent GameObject here
    public Button toggleButton; // The single toggle button
    public bool isOn = true;
    
    [HideInInspector] public string toggleID; // Will be set automatically
    
    // Individual events for this specific toggle
    public UnityEngine.Events.UnityEvent onThisToggleOn;
    public UnityEngine.Events.UnityEvent onThisToggleOff;
}

public class ToggleEffectsManager : MonoBehaviour
{
    [Header("Toggle List")]
    public List<ToggleEffect> toggleEffects = new List<ToggleEffect>();
    
    [Header("Universal Sound Events")]
    public UnityEngine.Events.UnityEvent onAllToggleOn;
    public UnityEngine.Events.UnityEvent onAllToggleOff;
    
    [Header("PlayerPrefs Settings")]
    public string playerPrefsPrefix = "Toggle_";
    
    [Header("Button Colors")]
    public Color toggleOnColor = Color.green;
    public Color toggleOffColor = Color.red;
    
    [Header("Startup Behavior")]
    public bool triggerEventsOnStart = true;
    
    void Start()
    {
        InitializeAllToggles();
        LoadAllToggleStates();
        UpdateAllToggleVisuals();
        
        if (triggerEventsOnStart)
        {
            TriggerEventsOnStart();
        }
    }
    
    void OnDestroy()
    {
        SaveAllToggleStates();
    }
    
    void OnApplicationQuit()
    {
        SaveAllToggleStates();
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveAllToggleStates();
    }
    
    private void InitializeAllToggles()
    {
        foreach (var toggle in toggleEffects)
        {
            if (toggle.parentObject != null)
            {
                toggle.toggleID = toggle.parentObject.name;
                FindToggleButton(toggle);
                
                if (toggle.toggleButton != null)
                    toggle.toggleButton.onClick.AddListener(() => OnToggleClicked(toggle));
            }
        }
    }
    
    private void FindToggleButton(ToggleEffect toggle)
    {
        if (toggle.parentObject == null) return;
        
        // Look for any button in the parent object
        Button foundButton = toggle.parentObject.GetComponentInChildren<Button>();
        if (foundButton != null)
        {
            toggle.toggleButton = foundButton;
        }
        else
        {
            Debug.LogWarning($"No button found in children of {toggle.parentObject.name}");
        }
    }
    
    private void OnToggleClicked(ToggleEffect toggle)
    {
        // Toggle the state
        toggle.isOn = !toggle.isOn;
        UpdateToggleVisuals(toggle);
        SaveToggleState(toggle);
        
        TriggerToggleEvents(toggle, toggle.isOn);
    }
    
    private void TriggerToggleEvents(ToggleEffect toggle, bool turnOn)
    {
        if (turnOn)
        {
            onAllToggleOn?.Invoke();
            toggle.onThisToggleOn?.Invoke();
        }
        else
        {
            onAllToggleOff?.Invoke();
            toggle.onThisToggleOff?.Invoke();
        }
    }
    
    private void TriggerEventsOnStart()
    {
        foreach (var toggle in toggleEffects)
        {
            TriggerToggleEvents(toggle, toggle.isOn);
        }
    }
    
    private void UpdateToggleVisuals(ToggleEffect toggle)
    {
        if (toggle.toggleButton != null)
        {
            // Update button color based on state
            Image buttonImage = toggle.toggleButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = toggle.isOn ? toggleOnColor : toggleOffColor;
            }
            
            // Update text if exists
            Text buttonText = toggle.toggleButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = toggle.isOn ? "ON" : "OFF";
                buttonText.color = Color.white;
            }
        }
    }
    
    private void UpdateAllToggleVisuals()
    {
        foreach (var toggle in toggleEffects)
        {
            UpdateToggleVisuals(toggle);
        }
    }
    
    // PlayerPrefs Methods
    private void SaveToggleState(ToggleEffect toggle)
    {
        string key = playerPrefsPrefix + toggle.toggleID;
        PlayerPrefs.SetInt(key, toggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void LoadToggleState(ToggleEffect toggle)
    {
        string key = playerPrefsPrefix + toggle.toggleID;
        if (PlayerPrefs.HasKey(key))
        {
            toggle.isOn = PlayerPrefs.GetInt(key) == 1;
        }
    }
    
    public void SaveAllToggleStates()
    {
        foreach (var toggle in toggleEffects)
        {
            SaveToggleState(toggle);
        }
        PlayerPrefs.Save();
    }
    
    public void LoadAllToggleStates()
    {
        foreach (var toggle in toggleEffects)
        {
            LoadToggleState(toggle);
        }
    }
    
    public void ResetAllToggleStates()
    {
        foreach (var toggle in toggleEffects)
        {
            toggle.isOn = true;
            UpdateToggleVisuals(toggle);
        }
        SaveAllToggleStates();
        
        if (triggerEventsOnStart)
        {
            TriggerEventsOnStart();
        }
    }
    
    public void DeleteAllTogglePlayerPrefs()
    {
        foreach (var toggle in toggleEffects)
        {
            string key = playerPrefsPrefix + toggle.toggleID;
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
    }
    
    public void SetToggleState(int index, bool state, bool triggerEvents = true)
    {
        if (index >= 0 && index < toggleEffects.Count)
        {
            var toggle = toggleEffects[index];
            if (toggle.isOn != state)
            {
                toggle.isOn = state;
                UpdateToggleVisuals(toggle);
                SaveToggleState(toggle);
                
                if (triggerEvents)
                {
                    TriggerToggleEvents(toggle, state);
                }
            }
        }
    }
    
    public void SetToggleStateByID(string toggleID, bool state, bool triggerEvents = true)
    {
        var toggle = toggleEffects.Find(t => t.toggleID == toggleID);
        if (toggle != null && toggle.isOn != state)
        {
            toggle.isOn = state;
            UpdateToggleVisuals(toggle);
            SaveToggleState(toggle);
            
            if (triggerEvents)
            {
                TriggerToggleEvents(toggle, state);
            }
        }
    }
    
    public bool GetToggleState(int index)
    {
        if (index >= 0 && index < toggleEffects.Count)
            return toggleEffects[index].isOn;
        return false;
    }
    
    public bool GetToggleStateByID(string toggleID)
    {
        var toggle = toggleEffects.Find(t => t.toggleID == toggleID);
        return toggle?.isOn ?? false;
    }
    
    public void TriggerAllEventsNow()
    {
        foreach (var toggle in toggleEffects)
        {
            TriggerToggleEvents(toggle, toggle.isOn);
        }
    }
    
    [ContextMenu("Auto-Setup All Toggles")]
    public void AutoSetupAllToggles()
    {
        foreach (var toggle in toggleEffects)
        {
            if (toggle.parentObject != null)
            {
                FindToggleButton(toggle);
                toggle.toggleID = toggle.parentObject.name;
            }
        }
        Debug.Log("Auto-setup completed for all toggles");
    }
}