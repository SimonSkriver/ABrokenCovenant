using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FogPostProcessEffect : MonoBehaviour
{
    [SerializeField] DrainEffect drainZone;

    [Header ("Volume Profile")]
    [SerializeField] private VolumeProfile volume;

    [Header ("Heartbeat SFX")]
    [SerializeField] private AudioSource heartbeatSFX;
    [SerializeField] private float heartbeatMax = 1f;

    [Header ("Vignette")]
    [SerializeField] private float vignetteMax = 1f;
    private Vignette vignette;

    [Header ("Film Grain")]
    [SerializeField] private float filmGrainIntensityMax = 1f;
    private FilmGrain filmGrain;
    
    [Header ("Chromatic aberration")]
    [SerializeField] private float chromaticAbIntensityMax = 1f;
    private ChromaticAberration chromaticAberration;

    [SerializeField] private bool effectsShouldPlay;

    void Start()
    {
        volume.TryGet(out vignette);
        volume.TryGet(out filmGrain);
        volume.TryGet(out chromaticAberration);
    }

    void Update()
    {
        float t = drainZone.counter/drainZone.maxTime;
        if (t > 0) effectsShouldPlay = true;
        HandleEffects(t);
        if (t == 0) effectsShouldPlay = false;
    }

    void HandleEffects(float t)
    {   
        if (effectsShouldPlay) 
        {
            if (heartbeatSFX != null) heartbeatSFX.volume = Mathf.Lerp(0, heartbeatMax, t);
            vignette.intensity.value = Mathf.Lerp(0, vignetteMax, t);
            filmGrain.intensity.value = Mathf.Lerp(0, filmGrainIntensityMax, t);
            chromaticAberration.intensity.value = Mathf.Lerp(0, chromaticAbIntensityMax, t);
        }
    }
}