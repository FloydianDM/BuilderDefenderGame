using UnityEngine;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private ResourceTypeSO _goldResource;

    private ResourceManager _resourceManager;
    private TooltipUI _tooltipUI;
    
    private void Start()
    {
        _resourceManager = FindAnyObjectByType<ResourceManager>();
        _tooltipUI = FindAnyObjectByType<TooltipUI>(FindObjectsInactive.Include);
    }

    public void RepairBuilding()
    {
        SpendSomeResources();
        
    }

    private void SpendSomeResources()
    {
        int lostHealth = _healthSystem.MaxHealthAmount - _healthSystem.HealthAmount;

        int repairCost = Mathf.FloorToInt((float)lostHealth / 2);

        var repairResourceAmountArray = new ResourceAmount[1];

        repairResourceAmountArray[0] = new ResourceAmount
        {
            ResourceType = _goldResource,
            Amount = repairCost
        };

        if (!_resourceManager.CanAfford(repairResourceAmountArray))
        {
            _tooltipUI.ShowTooltipText("Cannot afford the repairment!");
            
            return;
        }
        
        _healthSystem.Heal();
        _resourceManager.SpendResources(repairResourceAmountArray);
    }
}
