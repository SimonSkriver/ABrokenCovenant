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
    private InputActionMap playerInputMap;

    [Header("Sensitivity settings")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("Checks")]
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
        
        if (escapeAction != null)
        {
            escapeAction.Enable();
            escapeAction.performed += ctx => ShowAndHidePauseScreen();
        }
        if (playerCameraMovement != null)
        {
            sensitivitySlider.value = playerCameraMovement.GetSensitivity();
        }
    }

    public void ShowAndHidePauseScreen()
    {
        if(!escapeMenuShown) 
        {   
            DisableExternalSFXs();
            DisableSounds();
            savedPlayerState = PlayerMovement.Instance.currentState;
            PlayerMovement.Instance.currentState = PlayerState.LockPlayer;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            escapeMenuShown = true;
            mouseSensPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else if(escapeMenuShown)
        {
            EnableSounds();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Ensure revert panel if player exits to game while in volume settings via Escape
            volumeSettingsPanel.SetActive(false);
            volumeSettingsShown = false;

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
            EnableSounds();
        }
    }

    public void ShowAndHideVolumeSettings()
    {
        if(!volumeSettingsShown)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EnableSounds();
            mouseSensPanel.SetActive(false);
            volumeSettingsPanel.SetActive(true);
            volumeSettingsShown = true;
        }
        else if(volumeSettingsShown)
        {  
            volumeSettingsPanel.SetActive(false);
            mouseSensPanel.SetActive(true);
            volumeSettingsShown = false;
        }
    }

    public void OnSensitivityChanged(float value)
    {
        if (playerCameraMovement != null)
        {
            playerCameraMovement.SetSensitivity(value);
        }
    }

    private void DisableSounds()
    {
        savedMasterVolume = AudioManager.Instance.masterVolume;
        AudioManager.Instance.SetMasterVolume(-80f);
    }

    private void EnableSounds()
    {
        AudioManager.Instance.SetMasterVolume(savedMasterVolume);
    }

    private void DisableExternalSFXs()
    {
        PlayerMovement.Instance.StopSFX();
        if (puzzleRotater != null) puzzleRotater.StopSFX();
    }

    public void SetActivePuzzleScript(PuzzleRotater puzzleScript)
    {
        puzzleRotater = puzzleScript;
    }
}