using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingSelectUI _buildingSelectUI;
    [SerializeField] private Building _hqBuilding;
    [SerializeField] private ResourceManager _resourceManager;

    private TooltipUI _tooltipUI;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    public event Action<BuildingTypeSO> OnActiveBuildingChanged;
    public event Action OnHQBuildingDown; 

    private void Awake()
    {
        _buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypes");
    }

    private void Start()
    {
        _tooltipUI = FindFirstObjectByType<TooltipUI>();
        
        BuilderDefenderGameInputActions inputActions = new();
        
        inputActions.Player.Enable();
        inputActions.Player.Select.performed += PerformClick;
        inputActions.Player.SelectPreviousBuildingType.performed += SelectPreviousBuilding;
        inputActions.Player.SelectNextBuildingType.performed += SelectNextBuilding;
        
        GetHQHealth().OnGameOver += HandleHQBuildingDown;
    }
    
    private void PerformClick(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (_activeBuildingType == null)
        {
            return;
        }

        if (!_resourceManager.CanAfford(_activeBuildingType.ConstructionResourceCostArray))
        {
            _tooltipUI.ShowTooltipText("Cannot afford the building! " + 
                                               _activeBuildingType.GetConstructionResourceCostString());
            
            return;
        }

        if (!CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
        {
            _tooltipUI.ShowTooltipText(errorMessage);
            
            return;
        }
        
        BuildingConstruction.CreateBuildingConstruction(UtilsClass.GetMouseWorldPosition(), _activeBuildingType);
        
        _resourceManager.SpendResources(_activeBuildingType.ConstructionResourceCostArray);
    }

    private void SelectPreviousBuilding(InputAction.CallbackContext context)
    {
        _activeBuildingType = _buildingTypeList.BuildingTypes[0];
    }

    private void SelectNextBuilding(InputAction.CallbackContext context)
    {
        _activeBuildingType = _buildingTypeList.BuildingTypes[1];
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        _activeBuildingType = buildingType;
        _buildingSelectUI.SetActiveBuildingButton(_activeBuildingType);

        OnActiveBuildingChanged?.Invoke(_activeBuildingType);
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCol = buildingType.Prefab.GetComponent<BoxCollider2D>();
        Collider2D[] colliderArray = Physics2D.OverlapBoxAll(
            position + (Vector3)boxCol.offset, boxCol.size, 0);

        bool isAreaClear = colliderArray.Length == 0;

        if (!isAreaClear)
        {
            errorMessage = "Area isn't clear";
            return false;
        }
        
        colliderArray = Physics2D.OverlapCircleAll(position, buildingType.MinConstructionRadius);

        foreach (Collider2D col in colliderArray)
        {
            // Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = col.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                if (buildingTypeHolder.BuildingType == buildingType)
                {
                    // There's already a building in same type

                    errorMessage = "There is another building around for same resource!";
                    return false;
                }
            }
        }
        
        // Max distance between any buildings to prevent overuse of the map

        float maxConstructionRadius = 25f;
        
        colliderArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D col in colliderArray)
        {
            // Colliders inside the construction radius
           
            BuildingTypeHolder buildingTypeHolder = col.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                // It's a building

                errorMessage = null;
                return true;
            }
        }

        errorMessage = "Too far from other buildings!";
        return false;
    }
    
    private void HandleHQBuildingDown()
    {
        OnHQBuildingDown?.Invoke();
    }

    public Building GetHqBuilding()
    {
        return _hqBuilding;
    }

    private HealthSystem GetHQHealth()
    {
        HealthSystem hqHealth = _hqBuilding.GetComponent<HealthSystem>();

        return hqHealth;
    }
}
