using UnityEngine;
using UnityEngine.Rendering;

public class ChromaticAberrationEffect : MonoBehaviour
{
    public static ChromaticAberrationEffect Instance;
    
    private Volume _chromaticAberrationVolume;

    private void Awake()
    {
        ManageSingleton();
        
        _chromaticAberrationVolume = GetComponent<Volume>();
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

    private void Update()
    {
        DecreaseChromaticAberrationVolume();
    }

    private void DecreaseChromaticAberrationVolume()
    {
        if (_chromaticAberrationVolume.weight > 0)
        {
            _chromaticAberrationVolume.weight -= 0.2f * Time.deltaTime;
        }
    }

    public void SetChromaticAberrationWeight(float weight)
    {
        _chromaticAberrationVolume.weight = weight;
    }
    
}
