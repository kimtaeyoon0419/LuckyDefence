// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngineInternal;

[System.Serializable]
public class AudioInfo
{
    public string audioName;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioInfo> bgmList;
    public List<AudioInfo> sfxList;

    public AudioSource audioSource;

    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> sfxDict;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitializeDictionaries();
        audioSource.loop = true;
        StartBGM("Stage");
    }


    private void InitializeDictionaries()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (AudioInfo info in bgmList)
        {
            if (!bgmDict.ContainsKey(info.audioName))
                bgmDict.Add(info.audioName, info.audioClip);
        }

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (AudioInfo info in sfxList)
        {
            if (!sfxDict.ContainsKey(info.audioName))
                sfxDict.Add(info.audioName, info.audioClip);
        }
    }

    public void StartBGM(string bgmName)
    {
        if (bgmDict.TryGetValue(bgmName, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM not found: " + bgmName);
        }
    }

    public void StartSfx(string sfxName)
    {
        if (sfxDict.TryGetValue(sfxName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + sfxName);
        }
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PauseBGM()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ResumeBGM()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
