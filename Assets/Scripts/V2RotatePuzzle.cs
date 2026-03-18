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
    [SerializeField] private float currentRotation;
    [SerializeField] private float oldRotation;
    [SerializeField] private float rotationSFXTrigger = 0.05f;
    [SerializeField] private float stopDelay = 0.1f;
    [SerializeField] private float lastMoveTime;
    
    [SerializeField] private AudioSource rotateSFX;
    
    void Awake()
    {
        horizontalAim = GameObject.FindGameObjectWithTag("Player").transform;
        verticalAim = GameObject.FindGameObjectWithTag("PlayerCam").transform;

        currentRotation = horizontalPivot.eulerAngles.y;
        oldRotation = currentRotation;
    }

    void Update()
    {
        if (PlayerMovement.Instance.currentState == PlayerState.InPuzzle && activePuzzle == this)
        {
            DoPuzzle();
            PlayMirrorRotatingSFX();
        }
    }

    public void Interact(GameObject obj)
    {
        lastMoveTime = Time.time;
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
        oldRotation = horizontalPivot.eulerAngles.y;
        //Rotate the mirror based on camera and player rotation
        horizontalPivot.rotation = Quaternion.Euler(horizontalPivot.eulerAngles.x, horizontalAim.eulerAngles.y, horizontalPivot.eulerAngles.z);
        verticalPivot.rotation = Quaternion.Euler(verticalAim.eulerAngles.x, verticalPivot.eulerAngles.y, verticalPivot.eulerAngles.z);
        currentRotation = horizontalPivot.eulerAngles.y;
    }

    public void Drop()
    {
        activePuzzle = null;
        PlayerMovement.Instance.currentState = PlayerState.Normal;
        if (rotateSFX.isPlaying) rotateSFX.Stop();
    }

    private void PlayMirrorRotatingSFX()
    {
        float changeValue = Mathf.Abs(Mathf.DeltaAngle(oldRotation, currentRotation));

        if (changeValue > rotationSFXTrigger)
        {
            lastMoveTime = Time.time;
        
            if (!rotateSFX.isPlaying)
            {
                rotateSFX.Play();
            }
        }
        else if (rotateSFX.isPlaying && Time.time - lastMoveTime > stopDelay)
        {
            rotateSFX.Stop();
        }
    }
}