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
            StartCoroutine(PlaySFX());
        }
    }

    IEnumerator PlaySFX()
    {
        if (InternalPlayerWhisper.Instance != null && InternalPlayerWhisper.Instance.internalWhisperSource != null)
        {
          if (InternalPlayerWhisper.Instance.internalWhisperSource.isPlaying)
            {
            yield return new WaitForSeconds(InternalPlayerWhisper.Instance.currentInternalWhisperLength);
            }

            InternalPlayerWhisper.Instance.isHearingHouseChatter = true;
            whisperSFX.Play();
            sfxHasPlayed = true;

            yield return new WaitForSeconds(whisperSFX.clip.length);
            InternalPlayerWhisper.Instance.isHearingHouseChatter = false;  
        }
        
    }
}
