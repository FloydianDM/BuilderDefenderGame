using System;
using TMPro;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _sFXVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private AudioManager _audioManager;

    public event Action OnEdgeScrollingToggleChanged; 

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        SetVolumeText();
    }
    
    public void ToggleOptionsUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeInHierarchy)
        {
            Time.timeScale = 0f; // Pause game
        }
        else
        {
            Time.timeScale = 1f; // Resume game
        }
    }

    public void ToggleEdgeScrolling()
    {
        OnEdgeScrollingToggleChanged?.Invoke();
    }

    public void DecreaseSFX()
    {
        _audioManager.DecreaseSFXVolume();
        SetVolumeText();
    }

    public void IncreaseSFX()
    {
        _audioManager.IncreaseSFXVolume();
        SetVolumeText();
    }

    public void DecreaseMusic()
    {
        _audioManager.DecreaseMusicVolume();
        SetVolumeText();
    }

    public void IncreaseMusic()
    {
        _audioManager.IncreaseMusicVolume();
        SetVolumeText();
    }

    private void SetVolumeText()
    {
        _sFXVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.SFXVolume * 10).ToString();
        _musicVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.MusicVolume * 10).ToString();
    }
}
