using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalPlayerWhisper : MonoBehaviour
{
    public static InternalPlayerWhisper Instance;

    public AudioSource internalWhisperSource {get; private set; }
    public bool isHearingHouseChatter {get; set;}


    //Keeps track of the current whisper length to use in RandomHouseChatter
    public float currentInternalWhisperLength {get; private set; }

    [SerializeField] private float timer;
    [SerializeField] private float timeForNextSFX;
    [SerializeField] private bool isPlayingSFX;
    [SerializeField] private float minDelay = 500f;
    [SerializeField] private float maxDelay = 1000f;
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
        else
        {
            Destroy(gameObject);
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

    if (internalWhisperSource != null)
        {
        if (!isPlayingSFX && timer > timeForNextSFX) 
            {
            PlaySFXByInsanityLevel();
            }
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
        isPlayingSFX = true;
        while (isHearingHouseChatter) //Safety check to not hear double voices if you're currently hearing voices coming from a house
        {
            yield return null; //if you are wait for that sound effect to finish playing
        }

            if (nextVoiceClipToPlay < audioClips.Count) //Check that we didnt go out of bounds
            {
                AudioClip clipToPlay = audioClips[nextVoiceClipToPlay];

                if (clipToPlay != null)
                {
                currentInternalWhisperLength = clipToPlay.length;
                internalWhisperSource.PlayOneShot(clipToPlay);
                yield return new WaitForSeconds(clipToPlay.length);
                nextVoiceClipToPlay++;
                }
            }
        if (nextVoiceClipToPlay >= audioClips.Count) nextVoiceClipToPlay = 0; //Go back to first audioclip if we reached the last one in the list

        timeForNextSFX = timer + UnityEngine.Random.Range(minDelay, maxDelay);
        isPlayingSFX = false;
    }
}

