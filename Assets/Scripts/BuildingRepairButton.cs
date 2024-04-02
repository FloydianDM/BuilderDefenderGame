using UnityEngine;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;

    public void RepairBuilding()
    {
        SpendSomeResources();
        _healthSystem.Heal();
    }

    private void SpendSomeResources()
    {
        BuildingTypeHolder buildingTypeHolder = _healthSystem.GetComponent<BuildingTypeHolder>();
        BuildingTypeSO buildingType = buildingTypeHolder.BuildingType;
        ResourceAmount[] resourceCostArray = buildingType.ConstructionResourceCostArray;

        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            FindAnyObjectByType<ResourceManager>().AddResource(
                resourceCost.ResourceType, -Mathf.FloorToInt((float)resourceCost.Amount / 2));
        }
    }
}
