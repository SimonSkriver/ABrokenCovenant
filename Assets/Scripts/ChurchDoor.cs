using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChurchDoor : MonoBehaviour
{
    [SerializeField] private InputAction escapeAction;
    [SerializeField] private GameObject gameOverPanel;
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
            DisableEscapeButton();
            gameOverPanel.SetActive(true);
            whiteAnimator = GameObject.FindGameObjectWithTag("FadeToWhite").GetComponentInChildren<Animator>(true);
            textAnimator = GameObject.FindGameObjectWithTag("Text").GetComponentInChildren<Animator>(true);
            PlayerMovement.Instance.currentState = PlayerState.LockPlayer;
            whiteAnimator.SetTrigger("FadeToWhite");
            Invoke("ShowText", 2f);
            Debug.Log("You won");
        }
    }

    void ShowText()
    {
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