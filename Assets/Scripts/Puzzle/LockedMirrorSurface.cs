using UnityEngine;

public class LockedMirrorSurface : MonoBehaviour
{
    [SerializeField] private Transform beamEmitter; // GameObject on prefab used to customize next start and endpoint of next RayCast hit.
    [SerializeField] private AudioSource mirrorSFXSource; // AudioSource that plays the mirror hit sfx 

    // Public get for MultiBeamScript to access Transform
    public Transform GetEmitterTransform()
    {
        return beamEmitter;
    }

    // Public method for MultiBeamScript to activate SFX on mirror hit
    public void PlayHitSFX()
    {
        if (mirrorSFXSource != null && !mirrorSFXSource.isPlaying)
        {
            mirrorSFXSource.Play();   
        }
    }
}