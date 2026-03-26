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
    [SerializeField] private float vignetteMin = 0.05f;
    [SerializeField] private float vignetteMax = 1f;
    private Vignette vignette;

    [Header ("Film Grain")]
    [SerializeField] private float filmGrainIntensityMin = 0.1f;
    [SerializeField] private float filmGrainIntensityMax = 1f;
    private FilmGrain filmGrain;
    
    [Header ("Chromatic aberration")]
    [SerializeField] private float chromaticAbIntensityMin = 0f;
    [SerializeField] private float chromaticAbIntensityMax = 1f;
    private ChromaticAberration chromaticAberration;

    [SerializeField] private bool effectsShouldPlay;

    void Start()
    {
        //TryGet the three different effects and save them in their respectiv variables
        volume.TryGet(out vignette);
        volume.TryGet(out filmGrain);
        volume.TryGet(out chromaticAberration);
    }

    void Update()
    {
        //Normalize the counter since post process effects have a value of 0-1
        float t = drainZone.counter/drainZone.maxTime;
        //Check if sfx should be playing
        if (t > 0) effectsShouldPlay = true;
        if (t == 0) effectsShouldPlay = false;
        HandleEffects(t);
    }

    void HandleEffects(float t)
    {   
        if (effectsShouldPlay) 
        {
            //Lerp from min value to max value using the counter as the parameter
            if (heartbeatSFX != null) heartbeatSFX.volume = Mathf.Lerp(0, heartbeatMax, t);
            vignette.intensity.value = Mathf.Lerp(vignetteMin, vignetteMax, t);
            filmGrain.intensity.value = Mathf.Lerp(filmGrainIntensityMin, filmGrainIntensityMax, t);
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAbIntensityMin, chromaticAbIntensityMax, t);
        }
    }
}