using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; // New Input System

[System.Serializable]
public class CheatCode
{
    [Header("Cheat Info")]
    [Tooltip("The keyword to trigger this cheat after typing 'cheat'")]
    public string code;

    [Tooltip("Description of what the cheat does")]
    public string description;

    [Header("Cheat Action")]
    [Tooltip("The action that happens when this cheat is activated")]
    public UnityEvent onCheatActivated;
}

public class CheatCodeConsole : MonoBehaviour
{
    [Header("Cheat Settings")]
    [Tooltip("List of all cheat codes available in the game")]
    public List<CheatCode> cheatCodes = new List<CheatCode>();

    private string inputBuffer = "";
    private bool cheatModeActivated = false;
    private string cheatKeyword = "cheat";

    void Update()
    {
        // Loop through all keys in the current keyboard
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                // Backspace
                if (key == Keyboard.current.backspaceKey && inputBuffer.Length > 0)
                {
                    inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                    continue;
                }

                // Enter = Submit the input
                if (key == Keyboard.current.enterKey)
                {
                    ProcessInput();
                    inputBuffer = "";
                    continue;
                }

                // Accept letters and numbers only
                if (key.displayName.Length == 1 && char.IsLetterOrDigit(key.displayName[0]))
                {
                    inputBuffer += key.displayName.ToLower();
                }
            }
        }

        // Activate cheat mode
        if (!cheatModeActivated && inputBuffer.ToLower().EndsWith(cheatKeyword))
        {
            Debug.Log("✅ Cheat mode activated. Now type your cheat code and press Enter.");
            cheatModeActivated = true;
            inputBuffer = "";

            foreach (var cheat in cheatCodes)
            {
                Debug.Log($"👉 Available: {cheat.code} - {cheat.description}");
            }
        }
    }

    private void ProcessInput()
    {
        if (!cheatModeActivated)
            return;

        string typedCode = inputBuffer.ToLower();
        foreach (var cheat in cheatCodes)
        {
            if (typedCode == cheat.code.ToLower())
            {
                Debug.Log($"✅ Cheat activated: {cheat.code} - {cheat.description}");
                cheat.onCheatActivated.Invoke();
                cheatModeActivated = false;
                return;
            }
        }

        Debug.Log($"❌ Invalid cheat code: {typedCode}");
        cheatModeActivated = false;
    }
}
