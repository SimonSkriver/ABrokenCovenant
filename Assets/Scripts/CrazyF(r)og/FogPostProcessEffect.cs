using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogPostProcessEffect : MonoBehaviour
{
    [SerializeField] DrainEffect drainZone;

    [Header ("Volume Profile")]
    [SerializeField] VolumeProfile volume;

    [Header ("Vignette")]
    [SerializeField] float vignetteMax = 1f;
    private Vignette vignette;

    [Header ("Film Grain")]
    [SerializeField] float filmGrainIntensityMax = 1f;
    private FilmGrain filmGrain;
    
    [Header ("Chromatic aberration")]
    [SerializeField] float chromaticAbIntensityMax = 1f;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        volume.TryGet(out vignette);
        volume.TryGet(out filmGrain);
        volume.TryGet(out chromaticAberration);
    }

    void Update()
    {
        float t = drainZone.counter/drainZone.maxTime;
        HandleEffects(t);
    }

    void HandleEffects(float t)
    {
        vignette.intensity.value = Mathf.Lerp(0, vignetteMax, t);
        filmGrain.intensity.value = Mathf.Lerp(0, filmGrainIntensityMax, t);
        chromaticAberration.intensity.value = Mathf.Lerp(0, chromaticAbIntensityMax, t);
    }
}