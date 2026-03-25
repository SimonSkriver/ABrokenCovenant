using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // static Instance to access easily from other scripts.

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [Range(-20f, 20f)] public float masterVolume;
    [Range(-20f, 20f)] public float musicVolume;
    [Range(-20f, 20f)] public float sfxVolume;

    void Awake()
    {
        // Make the static instance a singleton, to ensure only one Instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Methods to set the different mixer groups volumes
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        audioMixer.SetFloat("MasterVolume", masterVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        audioMixer.SetFloat("SFXVolume", sfxVolume);
    }
}