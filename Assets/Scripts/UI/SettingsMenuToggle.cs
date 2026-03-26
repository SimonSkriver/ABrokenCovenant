using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenuToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject volumeSettingsPanel;
    [SerializeField] private GameObject mouseSensPanel;
    [SerializeField] private InputAction escapeAction;
    [SerializeField] private PlayerCameraMovement playerCameraMovement;
    [SerializeField] private PuzzleRotater puzzleRotater; 
    [SerializeField] private InputActionMap playerInputMap;

    [Header("Sensitivity settings")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("Checks")]
    
    // Used for internal handling of returning to game,
    // useful for the case of returning to game via Escape btn while in volume settings
    [SerializeField] private bool escapeMenuShown = false; 
    [SerializeField] private bool volumeSettingsShown = false;

    [Header("Internal Saves")]
    [SerializeField] private PlayerState savedPlayerState;
    [SerializeField] private float savedMasterVolume;

    void Awake()
    {
        playerInputMap = InputSystem.actions.FindActionMap("Player");
        playerInputMap.Enable();
        escapeAction = InputSystem.actions.FindAction("Escape");
        
        // if escapeAction isnt null bind the method ShowAndHidePauseScreen to it
        if (escapeAction != null)
        {
            escapeAction.Enable();
            escapeAction.performed += ctx => ShowAndHidePauseScreen();
        }

        // Ensure the sensitivity slider starts at the same value as initial sensitivity
        if (playerCameraMovement != null)
        {
            sensitivitySlider.value = playerCameraMovement.GetSensitivity();
        }
    }

    public void ShowAndHidePauseScreen()
    {
        // Shows the pause screen
        if(!escapeMenuShown) 
        {   
            // Disable sound effects
            DisableExternalSFXs();
            DisableSounds();

            // Safety meassure to make sure player returns to puzzle state if he paused while rotating a mirror
            savedPlayerState = PlayerMovement.Instance.currentState;
            PlayerMovement.Instance.currentState = PlayerState.LockPlayer;

            // Pause the game and show the panels
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            escapeMenuShown = true;
            mouseSensPanel.SetActive(true);

            // Enables the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Resume game
        else if(escapeMenuShown)
        {
            // Remove and lock the cursor to the game again
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Ensure volume panels are deactivated if player exits to game while in volume settings via Escape
            volumeSettingsPanel.SetActive(false);
            volumeSettingsShown = false;

            // Hide parent panel and start the game again with correct player state
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            escapeMenuShown = false;
            if (savedPlayerState == PlayerState.InPuzzle) 
            {
                PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
            }
            else
            {
               PlayerMovement.Instance.currentState = PlayerState.Normal; 
            }
            // Turn sounds back on
            EnableSounds();
        }
    }

    public void ShowAndHideVolumeSettings()
    {
        if(!volumeSettingsShown)
        {
            // Enable cursor 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Enable sounds for player to live adjust as wanted
            EnableSounds();

            // Disable sensitivity and other buttons but keep key guide
            mouseSensPanel.SetActive(false);

            // Enable volume panel
            volumeSettingsPanel.SetActive(true);
            volumeSettingsShown = true;
        }
        else if(volumeSettingsShown)
        {  
            // Revert back to main pause panel
            DisableSounds();
            volumeSettingsPanel.SetActive(false);
            mouseSensPanel.SetActive(true);
            volumeSettingsShown = false;
        }
    }

    public void OnSensitivityChanged(float value)
    {
        // Set the player camera sensitivty according to the slider value
        // Used in inspector as On Value Changed dynamic float
        if (playerCameraMovement != null)
        {
            playerCameraMovement.SetSensitivity(value);
        }
    }

    // Methods to disable and turn back sounds with the same level as 
    private void DisableSounds()
    {
        savedMasterVolume = AudioManager.Instance.masterVolume;
        AudioManager.Instance.SetMasterVolume(-80f);
    }

    private void EnableSounds()
    {
        AudioManager.Instance.SetMasterVolume(savedMasterVolume);
    }

    // Helper method to make sure rotate puzzle sfx stopped playing
    private void DisableExternalSFXs()
    {
        PlayerMovement.Instance.StopSFX();
        if (puzzleRotater != null) puzzleRotater.StopSFX();
    }

    // Public method called from the mirrors when players click interact
    // so this script knows what mirror to stop the SFX on
    public void SetActivePuzzleScript(PuzzleRotater puzzleScript)
    {
        puzzleRotater = puzzleScript;
    }

    // Public method for use in volumetoggleslider script to overwrite 
    // the initially saved master volume if changed in volume settings
    public void SetSavedMasterVolume(float volume)
    {
        savedMasterVolume = volume;
    }
}