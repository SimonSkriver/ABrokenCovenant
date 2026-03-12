using UnityEngine;

public class RotatePuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] Transform playerAnchor;
    [SerializeField] Transform horizontalPivot;
    [SerializeField] Transform horizontalAim;
    [SerializeField] Transform verticalPivot;
    [SerializeField] Transform verticalAim;
    [SerializeField] float degreesOfMotion = 45f;
    private float startY;

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
        Debug.Log("Doing puzzle");
        PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
        horizontalAim.rotation = Quaternion.LookRotation(horizontalPivot.right);
        verticalAim.rotation = Quaternion.LookRotation(verticalPivot.right);
        startY = horizontalAim.eulerAngles.y;
    }

    //Clamp virker ikke helt. Hvis man går ind og ud af puzzle, kan man cheese den. Clampen skal være fra første gang man interacter med den. Skal fikses. 
    //Skal også fikses, at man ikke kigger i den vej vertical pivot kigger, når man starter puzzle

    void DoPuzzle()
    {
        //Set player position to the anchor
        PlayerMovement.Instance.transform.position = playerAnchor.position;

        //Clamp the value of horizontal aim
        float clampedYRotation = horizontalAim.eulerAngles.y;
        clampedYRotation = Mathf.Clamp(clampedYRotation, startY - degreesOfMotion, startY + degreesOfMotion);
        horizontalAim.rotation = Quaternion.Euler(0, clampedYRotation, 0);

        //Rotate the mirror based on camera and player rotation
        horizontalPivot.rotation = Quaternion.Euler(horizontalPivot.eulerAngles.x, horizontalAim.eulerAngles.y - 90f, horizontalPivot.eulerAngles.z);
        verticalPivot.rotation = Quaternion.Euler(verticalPivot.eulerAngles.x, verticalPivot.eulerAngles.y, -verticalAim.eulerAngles.x);
    }

    public void Drop()
    {
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }
}