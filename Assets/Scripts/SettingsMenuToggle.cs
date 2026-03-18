using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenuToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject volumeSettingsPanel;
    [SerializeField] private GameObject mouseSensPanel;
    //[SerializeField] private GameObject buttonSettings;
    [SerializeField] private InputAction escapeAction;
    [SerializeField] private PlayerCameraMovement playerCameraMovement;

    [Header("Sensitivity settings")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("Checker boolians")]
    [SerializeField] private bool escapeMenuShown = false;
    [SerializeField] private bool volumeSettingsShown = false;

    void Awake()
    {
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
            pausePanel.SetActive(true);
            //buttonSettings.SetActive(false);
            Time.timeScale = 0f;
            escapeMenuShown = true;
        }

        else if(escapeMenuShown)
        {
            //Ensure revert panel if player exits to game while in volume settings via Escape
            volumeSettingsPanel.SetActive(false);
            volumeSettingsShown = false;

            pausePanel.SetActive(false);
            //buttonSettings.SetActive(true);
            Time.timeScale = 1f;
            escapeMenuShown = false;
        }
    }

    public void ShowAndHideVolumeSettings()
    {
        if(!volumeSettingsShown)
        {
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
}
