using UnityEngine;

public class ChurchDoor : MonoBehaviour
{
    private Animator whiteAnimator;
    private Animator textAnimator;

    void Start()
    {
        whiteAnimator = GameObject.FindGameObjectWithTag("FadeToWhite").GetComponentInChildren<Animator>(true);
        textAnimator = GameObject.FindGameObjectWithTag("Text").GetComponentInChildren<Animator>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
}