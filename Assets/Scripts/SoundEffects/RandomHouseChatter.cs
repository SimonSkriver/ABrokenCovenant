using System.Collections;
using UnityEngine;

public class RandomHouseChatter : MonoBehaviour
{
    [SerializeField] private AudioSource whisperSFX; // Set different audio clips to each RandomHouseChatter in Inspector
    [SerializeField] private bool sfxHasPlayed;

    // Play SFX if player enter triggerzone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayHouseChatterSFX();
        }
    }

    // After null check and if its not already been triggered, play the audioclip
    void PlayHouseChatterSFX()
    {
        if (whisperSFX != null && !sfxHasPlayed)
        {
            whisperSFX.Play();
            sfxHasPlayed = true;
        }
    }
}