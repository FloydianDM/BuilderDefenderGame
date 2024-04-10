using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;

    private Light2D _light2D;
    private float _clock;
    private float _timeSpeed;
    private float _dayLength = 20f;
    
    private void Awake()
    {
        _light2D = GetComponent<Light2D>();
        _timeSpeed = 1 / _dayLength;
    }

    private void Update()
    {
        ExecuteDayTimer();
    }

    private void ExecuteDayTimer()
    {
        _clock += Time.deltaTime * _timeSpeed;
        
        float fullDayColorCycleLength = 1f;
        _light2D.color = _gradient.Evaluate(_clock % fullDayColorCycleLength); // sky color
    }
}
