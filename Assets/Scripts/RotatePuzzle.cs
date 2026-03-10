using UnityEngine;

public class RotatePuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] Transform playerAnchor;
    [SerializeField] Transform horizontalPivot;
    [SerializeField] Transform verticalPivot;
    [SerializeField] Transform horizontalAim;
    [SerializeField] Transform verticalAim;
    private float initialLookDir;
    private PlayerInteract playerInteract;
    
    void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
    }

    void Update()
    {
        if (PlayerMovement.Instance.currentState == PlayerState.InPuzzle)
        {
            DoPuzzle();
        }
    }

    public void Interact(Transform obj)
    {
        Debug.Log("Youre in puzzle");
        PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
        initialLookDir = horizontalAim.eulerAngles.y;
    }

    public void Drop()
    {
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }

    void DoPuzzle()
    {
        PlayerMovement.Instance.transform.position = playerAnchor.position;
        float horizontalRotation = horizontalAim.eulerAngles.y - initialLookDir;
        horizontalPivot.rotation = Quaternion.Euler(horizontalPivot.eulerAngles.x, horizontalRotation, horizontalPivot.eulerAngles.z);
        playerAnchor.SetParent(horizontalPivot);
    }
}