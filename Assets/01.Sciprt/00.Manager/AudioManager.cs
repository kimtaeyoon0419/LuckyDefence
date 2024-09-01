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

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

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
        InitializeDictionaries();
    }

    private void Start()
    {
        bgmAudioSource.loop = true;
        sfxAudioSource.loop = false;
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
            bgmAudioSource.clip = clip;
            bgmAudioSource.Play();
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
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + sfxName);
        }
    }

    public void StopBGM()
    {
        if (bgmAudioSource.isPlaying)
            bgmAudioSource.Stop();
    }

    public void PauseBGM()
    {
        if (bgmAudioSource.isPlaying)
            bgmAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        if (!bgmAudioSource.isPlaying)
            bgmAudioSource.Play();
    }

    public void BgmVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }
}
