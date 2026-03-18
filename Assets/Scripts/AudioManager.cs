using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer audioMixer;
   // [SerializeField] private AudioMixerGroup[] musicMixer CANN BE ADDED WITH FOREACH FUNCTION IF WE START ADDING SUBGROUPS
    [Range(-80f, 20f)] public float masterVolume;
    [Range(-80f, 20f)] public float musicVolume;
    [Range(-80f, 20f)] public float sfxVolume;

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

    void Start()
    {
        //IF WE WANT THE VOLUME TO START AT MIDDLE VOLUME
        /*
        masterVolume = -30f;
        musicVolume = -30f;
        sfxVolume = -30f; 
        */
    }

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
    
    public void UpdateVolumeGroup(AudioMixerGroup[] groups, float volume)
    {
        foreach (var group in groups)
        {
            Debug.Log(group);
           // audioMixer.SetFloat(group, volume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
