using UnityEngine;

public class FogClearSurface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject particleParent; // Set CrazyFogParticle GameObject in Inspector
    [SerializeField] private GameObject fogCollider;
    [SerializeField] private GameObject fogEffect; // Set CrazyFogEffect parent GameObject in Inspector
    [SerializeField] private ParticleSystem[] particleSystems;

    void Start()
    {
        // Collect all particle systems from parent
        particleSystems = particleParent.GetComponentsInChildren<ParticleSystem>();
    }

    public void EnableParticles()
    {
        // Enable loop and start particle effect
        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            main.loop = true;
            particle.Play(true);
        }
        // Enable collider and CrazyFogEffect GameObject
        fogCollider.SetActive(true);
        fogEffect.SetActive(true);
    }

    public void DisableParticles()
    {
        // Disable loop
        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            main.loop = false;
        }
        // Remove collider and disable CrazyFogEffect
        fogCollider.SetActive(false);
        fogEffect.SetActive(false);
    }
}