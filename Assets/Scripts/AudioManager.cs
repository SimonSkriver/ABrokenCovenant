using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer audioMixer;
   // [SerializeField] private AudioMixerGroup[] musicMixer CANN BE ADDED WITH FOREACH FUNCTION IF WE START ADDING SUBGROUPS
    [Range(0f, 1f)] public float masterVolume;
    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float sfxVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        audioMixer.SetFloat("Master", masterVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat("Music", musicVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        audioMixer.SetFloat("SFX", sfxVolume);
    }
    
    public void UpdateVolumeGroup(AudioMixerGroup[] groups, float volume)
    {
        foreach (var group in groups)
        {
            Debug.Log(group);
           // audioMixer.SetFloat(group, volume);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
