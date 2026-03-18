using UnityEngine;

public class V2RotatePuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] Transform playerAnchor;
    [SerializeField] Transform horizontalPivot;
    [SerializeField] Transform horizontalAim;
    [SerializeField] Transform verticalPivot;
    [SerializeField] Transform verticalAim;

    private static V2RotatePuzzle activePuzzle;

    [Header("SFX Trigger")]
    [SerializeField] float currentRotation;
    [SerializeField] float newRotationTrigger;
    [SerializeField] bool sfxPlaying = false;
    [SerializeField] AudioSource rotateSFX;
    
    void Awake()
    {
        horizontalAim = GameObject.FindGameObjectWithTag("Player").transform;
        verticalAim = GameObject.FindGameObjectWithTag("PlayerCam").transform;

        currentRotation = horizontalPivot.rotation.x;
        newRotationTrigger = currentRotation + 1;
    }

    void Update()
    {
        if (PlayerMovement.Instance.currentState == PlayerState.InPuzzle && activePuzzle == this)
        {
            DoPuzzle();
        }
    }

    public void Interact(GameObject obj)
    {
        activePuzzle = this;
        Debug.Log("Doing puzzle");
        PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
        horizontalAim.rotation = Quaternion.LookRotation(horizontalPivot.forward);
        verticalAim.rotation = Quaternion.LookRotation(verticalPivot.forward);
    }

    //Skal også fikses, at man ikke kigger i den vej vertical pivot kigger, når man starter puzzle

    void DoPuzzle()
    {
        //Set player position to the anchor
        PlayerMovement.Instance.transform.position = playerAnchor.position;

        //Rotate the mirror based on camera and player rotation
        horizontalPivot.rotation = Quaternion.Euler(horizontalPivot.eulerAngles.x, horizontalAim.eulerAngles.y, horizontalPivot.eulerAngles.z);
        verticalPivot.rotation = Quaternion.Euler(verticalAim.eulerAngles.x, verticalPivot.eulerAngles.y, verticalPivot.eulerAngles.z);
    }

    public void Drop()
    {
        activePuzzle = null;
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }

    private void PlayMirrorRotatingSFX()
    {
        if (currentRotation > newRotationTrigger || currentRotation < newRotationTrigger)
        {
            if (!sfxPlaying)
            {
                
            }
        }
    }
}