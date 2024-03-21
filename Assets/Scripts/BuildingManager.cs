using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingSelectUI _buildingSelectUI;
    
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    public event Action<BuildingTypeSO> OnActiveBuildingChanged;

    private void Awake()
    {
        _buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypes");
    }

    private void Start()
    {
        BuilderDefenderGameInputActions inputActions = new();
        
        inputActions.Player.Enable();
        inputActions.Player.Select.performed += PerformClick;
        inputActions.Player.SelectPreviousBuildingType.performed += SelectPreviousBuilding;
        inputActions.Player.SelectNextBuildingType.performed += SelectNextBuilding;
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

        if (!ResourceManager.Instance.CanAfford(_activeBuildingType.ConstructionResourceCostArray))
        {
            TooltipUI.Instance.ShowTooltipText("Cannot afford the building! " + 
                                               _activeBuildingType.GetConstructionResourceCostString());
            
            return;
        }

        if (!CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
        {
            TooltipUI.Instance.ShowTooltipText(errorMessage);
            
            return;
        }

        Instantiate(_activeBuildingType.Prefab, UtilsClass.GetMouseWorldPosition(), quaternion.identity);
        ResourceManager.Instance.SpendResources(_activeBuildingType.ConstructionResourceCostArray);
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

        foreach (var col in colliderArray)
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

        foreach (var col in colliderArray)
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

        errorMessage = "Too far from civilization!";
        return false;
    }
}
