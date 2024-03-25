using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeHolder _buildingType;
    private HealthSystem _healthSystem;

    private void Start()
    {
        _buildingType = GetComponent<BuildingTypeHolder>();
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.SetMaxHealthAmount(_buildingType.BuildingType.MaxHealthAmount, true);
        
        _healthSystem.OnDie += DestroyBuilding;
    }

    private void DestroyBuilding()
    {
        Destroy(gameObject);
    }
    
}
