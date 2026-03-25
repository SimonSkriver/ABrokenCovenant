using System.Collections;
using UnityEngine;

public class RandomHouseChatter : MonoBehaviour
{
    [SerializeField] private AudioSource whisperSFX;
    [SerializeField] private bool sfxHasPlayed;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayHouseChatterSFX();
        }
    }

    void PlayHouseChatterSFX()
    {
        if (whisperSFX != null && !sfxHasPlayed)
        {
            whisperSFX.Play();
            sfxHasPlayed = true;
        }
    }
}