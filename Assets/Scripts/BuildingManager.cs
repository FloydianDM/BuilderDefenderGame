using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private BuildingSelectUI _buildingSelectUI;
    
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    public event Action<BuildingTypeSO> OnActiveBuildingChanged;

    private void Awake()
    {
        _buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypes");
    }

    private void Start()
    {
        _mainCamera = Camera.main;
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
        
        Instantiate(_activeBuildingType.Prefab, UtilsClass.GetMouseWorldPosition(), quaternion.identity);
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
}
