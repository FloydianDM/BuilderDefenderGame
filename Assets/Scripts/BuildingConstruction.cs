using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float _maxConstructionTimer;
    private float _constructionTimer;
    private BuildingTypeSO _buildingType;
    private BoxCollider2D _boxCollider;
    private BuildingTypeHolder _buildingTypeHolder;
    private Material _buildingConstructionMaterial;
    private int _progressShaderID;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        _buildingConstructionMaterial = GetComponentInChildren<SpriteRenderer>().material;
        _progressShaderID = Shader.PropertyToID("_Progress");
    }

    private void Update()
    {
        ConstructBuilding();
    }

    private void SetBuilding(BuildingTypeSO buildingType)
    {
        _maxConstructionTimer = buildingType.ConstructionTime;
        _constructionTimer = _maxConstructionTimer;
        
        _buildingType = buildingType;
        _buildingTypeHolder.BuildingType = buildingType;
        
        _boxCollider.offset = buildingType.Prefab.GetComponent<BoxCollider2D>().offset;
        _boxCollider.size = buildingType.Prefab.GetComponent<BoxCollider2D>().size;
    }

    private void ConstructBuilding()
    {
        _constructionTimer -= Time.deltaTime;
        _buildingConstructionMaterial.SetFloat(_progressShaderID, 1 - GetConstructionTimerRatio());

        if (_constructionTimer <= 0)
        {
            Instantiate(_buildingType.Prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public float GetConstructionTimerRatio()
    {
        return _constructionTimer / _maxConstructionTimer;
    }

    public static BuildingConstruction CreateBuildingConstruction(Vector2 spawnPosition, BuildingTypeSO buildingType)
    {
        Transform buildingConstructionPrefab = Resources.Load<Transform>("BuildingConstructionPrefab");
        buildingConstructionPrefab.GetComponentInChildren<SpriteRenderer>().sprite = buildingType.Sprite;
        Transform buildingConstructionTransform =
            Instantiate(buildingConstructionPrefab, spawnPosition, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        
        buildingConstruction.SetBuilding(buildingType);

        return buildingConstruction;
    }
}
