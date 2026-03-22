using System;
using UnityEngine;

public class FogClearSurface : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject particleParent;
    [SerializeField] private GameObject fogCollider;
    [SerializeField] private ParticleSystem[] particleSystems;
    void Start()
    {
        particleSystems = particleParent.GetComponentsInChildren<ParticleSystem>();
    }

    public void EnableParticles()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            main.loop = true;
            particle.Play(true);
        }
        fogCollider.SetActive(true);
    }

    public void DisableParticles()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            var main = particle.main;
            main.loop = false;
        }
        fogCollider.SetActive(false);
    }
}
