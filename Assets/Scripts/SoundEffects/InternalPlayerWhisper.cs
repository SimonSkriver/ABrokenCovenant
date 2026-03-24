using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalPlayerWhisper : MonoBehaviour
{
    public static InternalPlayerWhisper Instance;

    public AudioSource internalWhisperSource {get; private set; }
    public bool isHearingHouseChatter {get; set;}
    public float houseChatterSFXlength {get; set; }
    [SerializeField] private float timer;
    [SerializeField] private float timeForNextSFX;
    [SerializeField] private int insanityLevel = 1;
    [SerializeField] private int nextVoiceClipToPlay = 0;
    [SerializeField] private List<AudioClip> level1AudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> level2AudioClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> level3AudioClips = new List<AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        timeForNextSFX = 60;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        internalWhisperSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        UpdateInsanityLevel ();

        if (timer > timeForNextSFX) 
        {
            PlaySFXByInsanityLevel();
        }
    }
    void UpdateInsanityLevel()
    {
        //Increases insanityLevel every 5 minutes
        if (insanityLevel == 1 && timer > 300)
        {
            insanityLevel = 2;
            nextVoiceClipToPlay = 0;
        }
        if (insanityLevel == 2 && timer > 600)
        {
            insanityLevel = 3;
            nextVoiceClipToPlay = 0;
        }
       /* if (insanityLevel == 3 && timer > 900)
        {
            insanityLevel = 4;
            nextVoiceClipToPlay = 0;
        }   */
    }    

    void PlaySFXByInsanityLevel()
    { 
        switch (insanityLevel)
        {
            case 1:
                StartCoroutine(PlayNextSFX(level1AudioClips));
                break;
            case 2:
                StartCoroutine(PlayNextSFX(level2AudioClips));
                break;
            case 3:
                StartCoroutine(PlayNextSFX(level3AudioClips));
                break;
        }
    }

    IEnumerator PlayNextSFX(List<AudioClip> audioClips)
    {
        if (nextVoiceClipToPlay <= audioClips.Count) 
        {
        AudioClip clipToPlay = audioClips[nextVoiceClipToPlay];

        if (isHearingHouseChatter) //Safety check to not hear double voices if you're currently hearing voices coming from a house
            {
            yield return new WaitForSeconds(houseChatterSFXlength); //if you are wait for that sound effect to finish playing
            }

        internalWhisperSource.PlayOneShot(clipToPlay);
        if (nextVoiceClipToPlay != audioClips.Count) nextVoiceClipToPlay ++;
        timeForNextSFX = timer += UnityEngine.Random.Range(30f, 150f);
        }
    }
    
}

