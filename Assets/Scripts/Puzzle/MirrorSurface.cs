using UnityEngine;

public class MirrorSurface : MonoBehaviour
{
    [SerializeField] private AudioSource mirrorSFXSource; // AudioSource that plays the mirror hit sfx 


    // Public method for MultiBeamScript to activate SFX on mirror hit
    public void PlayHitSFX()
    {
        if (mirrorSFXSource != null && !mirrorSFXSource.isPlaying)
        {
            mirrorSFXSource.Play();   
        }
    }
}