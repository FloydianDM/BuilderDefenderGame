using UnityEngine;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private Building _building;

    public void DemolishBuilding()
    {
        GatherSomeResources();
        Destroy(_building.gameObject, 0.1f);
    }

    private void GatherSomeResources()
    {
        BuildingTypeHolder buildingTypeHolder = _building.GetComponent<BuildingTypeHolder>();
        BuildingTypeSO buildingType = buildingTypeHolder.BuildingType;
        ResourceAmount[] resourceCostArray = buildingType.ConstructionResourceCostArray;

        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            FindAnyObjectByType<ResourceManager>().AddResource(
                resourceCost.ResourceType, Mathf.FloorToInt((float)resourceCost.Amount / 2));
        }
    }
}
