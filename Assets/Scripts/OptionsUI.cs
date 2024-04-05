using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Toggle _edgeScrollingToggle;    
    [SerializeField] private TextMeshProUGUI _sFXVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;

    public event Action OnEdgeScrollingToggleChanged; 

    private void Awake()
    {
        gameObject.SetActive(false);
        
        SetEdgeScrollingToggleIndicator();
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
    
    private void SetEdgeScrollingToggleIndicator()
    {
        if (PlayerPrefs.GetInt(CameraHandler.EDGE_SCROLLING, 1) == 0)
        {
            _edgeScrollingToggle.isOn = false;
        }
        else if (PlayerPrefs.GetInt(CameraHandler.EDGE_SCROLLING, 1) == 1)
        {
            _edgeScrollingToggle.isOn = true;
        }
    }

    public void ToggleEdgeScrolling()
    {
        if (_edgeScrollingToggle.isOn)
        {
            PlayerPrefs.SetInt(CameraHandler.EDGE_SCROLLING, 1);
        }
        else
        {
            PlayerPrefs.SetInt(CameraHandler.EDGE_SCROLLING, 0);
        }
        
        OnEdgeScrollingToggleChanged?.Invoke();
    }

    public void DecreaseSFX()
    {
        AudioManager.Instance.DecreaseSFXVolume();
        SetVolumeText();
    }

    public void IncreaseSFX()
    {
        AudioManager.Instance.IncreaseSFXVolume();
        SetVolumeText();
    }

    public void DecreaseMusic()
    {
        AudioManager.Instance.DecreaseMusicVolume();
        SetVolumeText();
    }

    public void IncreaseMusic()
    {
        AudioManager.Instance.IncreaseMusicVolume();
        SetVolumeText();
    }

    private void SetVolumeText()
    {
        _sFXVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.SFXVolume * 10).ToString();
        _musicVolumeText.text = Mathf.RoundToInt(AudioManager.Instance.MusicVolume * 10).ToString();
    }
}
