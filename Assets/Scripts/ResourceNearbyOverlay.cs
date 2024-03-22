using TMPro;
using UnityEngine;

public class ResourceNearbyOverlay : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceNearbyText;
    [SerializeField] private SpriteRenderer _icon;
    
    private ResourceGeneratorData _resourceGeneratorData;

    private void Update()
    {
        float resourceGenerationEfficiencyRatio = 
            (float)ResourceGenerator.GetNearbyResourceAmount(_resourceGeneratorData, transform.position) /
            _resourceGeneratorData.MaxResourceAmount;
        
        _resourceNearbyText.text = resourceGenerationEfficiencyRatio.ToString("P");
    }

    public void ShowOverlay(ResourceGeneratorData resourceGeneratorData)
    {
        _resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);
        
        _icon.sprite = _resourceGeneratorData.ResourceType.Sprite;
        
    }

    public void HideOverlay()
    {
        gameObject.SetActive(false);
    }
}
