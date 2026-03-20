using UnityEngine;

public class MirrorSurface : MonoBehaviour
{
    [SerializeField] private AudioSource mirrorSFXSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHitSFX()
    {
        if (mirrorSFXSource != null && !mirrorSFXSource.isPlaying)
        {
        mirrorSFXSource.Play();   
        }
    }
}
