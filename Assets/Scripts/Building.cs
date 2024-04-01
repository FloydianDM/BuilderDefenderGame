using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Transform _buildingDemolishButtonUI;
    
    private BuildingTypeHolder _buildingType;
    private HealthSystem _healthSystem;

    private void Start()
    {
        if (_buildingDemolishButtonUI != null)
        {
            _buildingDemolishButtonUI.gameObject.SetActive(false);
        }
        
        _buildingType = GetComponent<BuildingTypeHolder>();
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.SetMaxHealthAmount(_buildingType.BuildingType.MaxHealthAmount, true);
        
        _healthSystem.OnDie += DestroyBuilding;
    }

    private void DestroyBuilding()
    {
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        if (_buildingDemolishButtonUI == null) // HQ does not have demolish function!
        {
            return;
        }
        
        _buildingDemolishButtonUI.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (_buildingDemolishButtonUI == null)
        {
            return;
        }
        
        _buildingDemolishButtonUI.gameObject.SetActive(false);
    }
}
