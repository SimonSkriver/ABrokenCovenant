using UnityEngine;

public class ChurchDoor : MonoBehaviour, IInteractable
{
    private Animator whiteAnimator;
    private Animator textAnimator;

    void Start()
    {
        whiteAnimator = GameObject.FindGameObjectWithTag("FadeToWhite").GetComponent<Animator>();
        textAnimator = GameObject.FindGameObjectWithTag("Text").GetComponent<Animator>();
    }

    public void Interact(GameObject obj)
    {
        PlayerMovement.Instance.currentState = PlayerState.LockPlayer;
        whiteAnimator.SetTrigger("FadeToWhite");
        Invoke("ShowText", 2f);
        Debug.Log("You won");
    }

    void ShowText()
    {
        textAnimator.SetTrigger("ShowText");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Drop()
    {
        
    }
}