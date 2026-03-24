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
        if (InternalPlayerWhisper.Instance.internalWhisperSource.isPlaying)
        {
            yield return new WaitForSeconds(InternalPlayerWhisper.Instance.internalWhisperSource.clip.length);
        }
        InternalPlayerWhisper.Instance.isHearingHouseChatter = true;
        InternalPlayerWhisper.Instance.houseChatterSFXlength = whisperSFX.clip.length;
        whisperSFX.Play();
        sfxHasPlayed = true;
        InternalPlayerWhisper.Instance.isHearingHouseChatter = false;
    }
}
