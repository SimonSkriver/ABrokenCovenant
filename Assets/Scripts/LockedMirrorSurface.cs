using System;
using UnityEngine;

public class LockedMirrorSurface : MonoBehaviour
{
    [SerializeField] private Transform beamEmitter;
    [SerializeField] private AudioSource mirrorSFXSource;
    
    public Transform GetTransform()
    {
        return beamEmitter;
    }

     public void PlayHitSFX()
    {
        if (mirrorSFXSource != null && !mirrorSFXSource.isPlaying)
        {
        mirrorSFXSource.Play();   
        }
    }
}
