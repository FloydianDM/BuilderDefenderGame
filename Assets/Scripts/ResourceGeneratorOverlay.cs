using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private Transform _icon;
    [SerializeField] private Image _timerBar;
    [SerializeField] private TMP_Text _resourceGenerationText;

    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData = _resourceGenerator.GetResourceGeneratorData();
        
        _icon.gameObject.GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.ResourceType.Sprite;
        HandleTimeChanged();
        HandleGenerationRateChanged();
        
        _resourceGenerator.OnTimeChanged += HandleTimeChanged;
        _resourceGenerator.OnGenerationRateChanged += HandleGenerationRateChanged;
    }

    private void HandleTimeChanged()
    {
        _timerBar.fillAmount = _resourceGenerator.GetTimerNormalized();
    }

    private void HandleGenerationRateChanged()
    {
        _resourceGenerationText.text = _resourceGenerator.GetGeneratedResourceAmountPerSecond().ToString("F1");
    }
}
