using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicAudioSource;
    
    private AudioSource _sFXAudioSource;
    private Dictionary<SFX, AudioClip> _sFXDictionary = new();
    public float SFXVolume { get; private set; } = 0.5f;
    public float MusicVolume { get; private set; } = 0.5f;

    private void Awake()
    {
        ManageSingleton();
        
        _sFXAudioSource = GetComponent<AudioSource>();
        _sFXAudioSource.volume = SFXVolume;

        _musicAudioSource.volume = MusicVolume;

        foreach (SFX sound in Enum.GetValues(typeof(SFX)))
        {
            _sFXDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    private void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void DecreaseSFXVolume()
    { 
        SFXVolume -= 0.1f;
        SFXVolume = Mathf.Clamp01(SFXVolume);

        _sFXAudioSource.volume = SFXVolume;
    }

    public void IncreaseSFXVolume()
    {
        SFXVolume += 0.1f;
        SFXVolume = Mathf.Clamp01(SFXVolume);
        
        _sFXAudioSource.volume = SFXVolume;
    }
    
    public void DecreaseMusicVolume()
    {
        MusicVolume -= 0.1f;
        MusicVolume = Mathf.Clamp01(MusicVolume);

        _musicAudioSource.volume = MusicVolume;
    }
    
    public void IncreaseMusicVolume()
    {
        MusicVolume += 0.1f;
        MusicVolume = Mathf.Clamp01(MusicVolume);

        _musicAudioSource.volume = MusicVolume;;
    }
    
    public void PlaySFX(SFX sfxType)
    {
        _sFXAudioSource.PlayOneShot(_sFXDictionary[sfxType], SFXVolume);
    }

    public enum SFX
    {
        BuildingDamaged,
        BuildingDestroyed,
        BuildingPlaced,
        EnemyDie,
        EnemyHit,
        EnemyWaveStarting,
        GameOver
    }


    
}

