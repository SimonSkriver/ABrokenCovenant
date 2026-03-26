using UnityEngine;
using UnityEngine.InputSystem;

public class GameStopper : MonoBehaviour
{
    [SerializeField] InputAction escapeAction;
    [SerializeField] GameObject gameOverPanel;
    private Animator whiteAnimator;
    private Animator textAnimator;

    void Awake()
    {
        escapeAction = InputSystem.actions.FindAction("Escape");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Stop sound, disable escape button, lock player and enable gameOverPanel
            PlayerMovement.Instance.StopSFX();
            DisableEscapeButton();
            gameOverPanel.SetActive(true);
            PlayerMovement.Instance.currentState = PlayerState.LockPlayer;

            // Get the white screen and text animator component and set trigger
            whiteAnimator = GameObject.FindGameObjectWithTag("FadeToWhite").GetComponentInChildren<Animator>(true);
            textAnimator = GameObject.FindGameObjectWithTag("Text").GetComponentInChildren<Animator>(true);
            whiteAnimator.SetTrigger("FadeToWhite");

            // Showtext invokes 2 seconds later, to show text after the white screen
            Invoke("ShowText", 2f);
            Debug.Log("You won");
        }
    }

    void ShowText()
    {
        // Set trigger to show text and re-enable cursor
        textAnimator.SetTrigger("ShowText");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void DisableEscapeButton()
    {
        if (escapeAction != null)
        {
            escapeAction.Disable();
        }
    }
}