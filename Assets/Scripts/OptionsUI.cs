using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button _sFXDecreaseButton;
    [SerializeField] private Button _sFXIncreaseButton;
    [SerializeField] private Button _musicDecreaseButton;
    [SerializeField] private Button _musicIncreaseButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _sFXVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        //gameObject.SetActive(false);
    }

    private void Start()
    {
        SetVolumeText();
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
