using UnityEngine;

public class PuzzleRotater : MonoBehaviour, IInteractable
{
    [SerializeField] Transform playerAnchor;
    [SerializeField] Transform horizontalPivot;
    [SerializeField] Transform horizontalAim;
    [SerializeField] Transform verticalPivot;
    [SerializeField] Transform verticalAim;

    private static PuzzleRotater activePuzzle; // Used to make sure only one puzzles' methods are run

    [Header("SFX Trigger")] // Variables used to check if sound should be played
    [SerializeField] private float currentRotation;
    [SerializeField] private float oldRotation;
    [SerializeField] private float rotationSFXTrigger = 0.05f;
    [SerializeField] private float stopDelay = 0.1f;
    [SerializeField] private float lastMoveTime;
    
    [SerializeField] private AudioSource rotateSFX;

    [Header("Reference")]
    [SerializeField] private SettingsMenuToggle settingsMenuScript;
    
    void Awake()
    {
        // Canvas reference to be used for sound effects
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        settingsMenuScript = canvas.GetComponentInChildren<SettingsMenuToggle>();

        //Get player and camera rotation reference to be used to rotate the pivots
        horizontalAim = GameObject.FindGameObjectWithTag("Player").transform;
        verticalAim = GameObject.FindGameObjectWithTag("PlayerCam").transform;

        // Initial value check. Is calculated later on in sound
        currentRotation = horizontalPivot.eulerAngles.y;
        oldRotation = currentRotation;
    }

    void Update()
    {
        // Only handle sound and rotation logic, if this is the active instance of all the puzzles.
        if (PlayerMovement.Instance.currentState == PlayerState.InPuzzle && activePuzzle == this)
        {
            DoPuzzle();
            PlayMirrorRotatingSFX();
        }
    }

    public void Interact(GameObject obj)
    {
        settingsMenuScript.SetActivePuzzleScript(this);
        lastMoveTime = Time.time;
        activePuzzle = this; 
        Debug.Log("Doing puzzle");

        // Update playerstate and set players orientation to the mirrors' rotation
        PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
        horizontalAim.rotation = Quaternion.LookRotation(horizontalPivot.forward);
        verticalAim.rotation = Quaternion.LookRotation(verticalPivot.forward);
    }

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
        // Clear active puzzle, reset playerstate and stop sfx
        activePuzzle = null;
        PlayerMovement.Instance.currentState = PlayerState.Normal;
        if (rotateSFX.isPlaying) rotateSFX.Stop();
    }

    private void PlayMirrorRotatingSFX()
    {
        // Check if mirror has been moved and only play sfx if it has
        float changeValue = Mathf.Abs(Mathf.DeltaAngle(oldRotation, currentRotation)); 

        if (changeValue > rotationSFXTrigger)
        {
            lastMoveTime = Time.time;
        
            if (!rotateSFX.isPlaying)
            {
                rotateSFX.Play(); // Only play the sound if sound isn't already playing
            }
        }
        
        else if (rotateSFX.isPlaying && Time.time - lastMoveTime > stopDelay)
        {
            StopSFX();
        }
    }

    public void StopSFX()
    {
        rotateSFX.Stop();
    }
}