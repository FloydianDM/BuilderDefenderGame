using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private Transform _resourceTemplate;
    [SerializeField] private ResourceManager _resourceManager;

    private ResourceTypeListSO _resourcesTypeList;
    private Dictionary<ResourceTypeSO, Transform> _resourceTypeToTransformDict = new();

    private void Awake()
    {
        SetResourceUITypes();
    }
    
    private void Start()
    {
        UpdateResourceAmount();
        _resourceManager.OnResourceAmountChanged += UpdateResourceAmount;
    }

    private void SetResourceUITypes()
    {
        _resourcesTypeList = Resources.Load<ResourceTypeListSO>("ResourceTypes");

        _resourceTemplate.gameObject.SetActive(false);

        int resourceIndex = 0;
        float offsetAmount = -160f; // distance between resource UI elements
        
        // Set Images of Resources
        
        foreach (var resourceType in _resourcesTypeList.ResourceTypes)
        {
            Transform resourceTransform = Instantiate(_resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);
            
            resourceTransform.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(resourceIndex * offsetAmount, 0);

            resourceTransform.Find("Image").GetComponent<Image>().sprite = resourceType.Sprite;

            _resourceTypeToTransformDict[resourceType] = resourceTransform;

            resourceIndex++;
        }
    }
    
    private void UpdateResourceAmount()
    {
        // Set Amounts of Resources
        
        foreach (var resourceType in _resourcesTypeList.ResourceTypes)
        {
            Transform resourceTransform = _resourceTypeToTransformDict[resourceType];
            int resourceAmount = _resourceManager.GetResourceAmount(resourceType);

            resourceTransform.Find("Text").GetComponent<TextMeshProUGUI>().text = resourceAmount.ToString();
        }

    }
}