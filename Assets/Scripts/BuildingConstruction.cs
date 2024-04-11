using System.Collections;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    [SerializeField] private ParticleSystem _buildingPlacedParticles;
    
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
        _buildingPlacedParticles.Play();
    }

    private void Start()
    {
        StartCoroutine(ConstructBuildingRoutine());
    }

    private void Update()
    {
        _constructionTimer -= Time.deltaTime;
        _buildingConstructionMaterial.SetFloat(_progressShaderID, 1 - GetConstructionTimerRatio());
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

    private IEnumerator ConstructBuildingRoutine()
    {
        _buildingConstructionMaterial.SetFloat(_progressShaderID, 1 - GetConstructionTimerRatio());

        yield return new WaitForSeconds(_constructionTimer);
        
        Instantiate(_buildingType.Prefab, transform.position, Quaternion.identity);
        _buildingPlacedParticles.Play();
        
        Destroy(gameObject, 0.5f);
    }

    public float GetConstructionTimerRatio()
    {
        return _constructionTimer / _maxConstructionTimer;
    }

    public static BuildingConstruction CreateBuildingConstruction(
        GameObject buildingConstructionPrefab, Vector2 spawnPosition, BuildingTypeSO buildingType)
    {
        buildingConstructionPrefab.GetComponentInChildren<SpriteRenderer>().sprite = buildingType.Sprite;
        Transform buildingConstructionTransform =
            Instantiate(buildingConstructionPrefab.transform, spawnPosition, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        
        buildingConstruction.SetBuilding(buildingType);

        return buildingConstruction;
    }
}
