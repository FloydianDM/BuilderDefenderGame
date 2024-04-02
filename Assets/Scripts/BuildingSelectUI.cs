using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite _cursorSprite;
    [SerializeField] private Transform _buttonTemplate;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private List<BuildingTypeSO> _ignoredBuildingTypeList; // for the buildings that are built in the scene
    [SerializeField] private TooltipUI _tooltipUI;
    
    private BuildingTypeListSO _buildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> _buildingTypeToButtonTransformDict = new();
    private Transform _cursorTransform;
    private float _buttonOffsetAmount = 120; // distance between buttons
    private int _buildingIndex; // building order of the buttons

    private void Start()
    {
        SetBuildingButtons();
    }

    private void SetBuildingButtons()
    {
        _buttonTemplate.gameObject.SetActive(false);

        _buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypes");
        
        SetCursorButton();

        foreach (BuildingTypeSO buildingType in _buildingTypeList.BuildingTypes)
        {
            if (_ignoredBuildingTypeList.Contains(buildingType))
            {
                continue;
            }
            
            Transform buttonTransform = Instantiate(_buttonTemplate, transform);
            
            buttonTransform.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(_buttonTemplate.position.x + (_buildingIndex * _buttonOffsetAmount), 
                    _buttonTemplate.position.y);
            
            buttonTransform.Find("Image").GetComponent<Image>().sprite = buildingType.Sprite;
            buttonTransform.gameObject.SetActive(true);
            buttonTransform.Find("Selected").gameObject.SetActive(false);
            
            buttonTransform.GetComponent<Button>().onClick.AddListener(
                () => _buildingManager.SetActiveBuildingType(buildingType));

            MouseEnterExitEvents mouseEnterExitEvents = buttonTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += () => _tooltipUI.ShowTooltipTextForButton(
                    buildingType.NameString + "\n" + buildingType.GetConstructionResourceCostString());
            mouseEnterExitEvents.OnMouseExit += _tooltipUI.HideTooltip;
            
            _buildingTypeToButtonTransformDict[buildingType] = buttonTransform;
            
            _buildingIndex++;
        }
    }

    private void SetCursorButton()
    {
        _cursorTransform = Instantiate(_buttonTemplate, transform);
            
        _cursorTransform.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(_buttonTemplate.position.x + (_buildingIndex * _buttonOffsetAmount), 
                _buttonTemplate.position.y);
       
        _cursorTransform.Find("Image").GetComponent<Image>().sprite = _cursorSprite;
        _cursorTransform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -25);
        _cursorTransform.gameObject.SetActive(true);
        _cursorTransform.Find("Selected").gameObject.SetActive(false);
        
        _cursorTransform.GetComponent<Button>().onClick.AddListener(
            () => _buildingManager.SetActiveBuildingType(null));
        
        MouseEnterExitEvents mouseEnterExitEvents = _cursorTransform.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += () => _tooltipUI.ShowTooltipTextForButton("Cursor");
        mouseEnterExitEvents.OnMouseExit += _tooltipUI.HideTooltip;

        _buildingIndex++;
    }

    public void SetActiveBuildingButton(BuildingTypeSO buildingType)
    {
        _cursorTransform.Find("Selected").gameObject.SetActive(false);
        
        foreach (BuildingTypeSO type in _buildingTypeToButtonTransformDict.Keys)
        {
            _buildingTypeToButtonTransformDict[type].Find("Selected").gameObject.SetActive(false);
        }

        if (buildingType == null) // if arrow button selected
        {
            _cursorTransform.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            _buildingTypeToButtonTransformDict[buildingType].Find("Selected").gameObject.SetActive(true);
        }
    }
}
