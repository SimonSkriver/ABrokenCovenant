using UnityEngine;

public class MirrorSurface : MonoBehaviour
{
    [SerializeField] private AudioSource mirrorSFXSource;

    public void PlayHitSFX()
    {
        if (mirrorSFXSource != null && !mirrorSFXSource.isPlaying)
        {
            mirrorSFXSource.Play();   
        }
    }
}