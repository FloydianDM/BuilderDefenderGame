using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance;
    
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineMultiChannelPerlin;
    private float _timer;
    private float _timerMax;
    private float _startingShakeIntensity;

    private void Awake()
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
        
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        ExecuteCameraShake();
    }

    private void ExecuteCameraShake()
    {
        if (_timer <= _timerMax)
        {
            _timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(_startingShakeIntensity, 0f, _timer / _timerMax);
            _cinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float shakeIntensity, float shakeDuration)
    {
        _timerMax = shakeDuration;
        _timer = 0;
        _startingShakeIntensity = shakeIntensity;
        _cinemachineMultiChannelPerlin.m_AmplitudeGain = shakeIntensity;
    }
    
}
