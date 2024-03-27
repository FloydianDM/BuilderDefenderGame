using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemyWaveNumberText;
    [SerializeField] private TextMeshProUGUI _enemyWaveMessageText;
    [SerializeField] private RectTransform _enemySpawnPositionIndicator;
    [SerializeField] private RectTransform _enemyClosestPositionIndicator;

    private EnemyWaveManager _enemyWaveManager;
    private Camera _mainCamera;

    private void Awake()
    {
        HideEnemyWaveUI();
    }

    private void Start()
    {
        _enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();
        _mainCamera = Camera.main;
        
        _enemyWaveManager.OnEnemyWaveNumberChanged += SetEnemyWaveNumberText;
        _enemyWaveManager.OnEnemyNextWaveRemainingTimeChanged += SetEnemyWaveMessageText;
        _enemyWaveManager.OnEnemyWavePositionInitialised += SetNextWaveSpawnPointIndicator;
        _enemyWaveManager.OnWaveFinished += HideEnemyWaveUI;
    }

    private void Update()
    {
        SetClosestEnemyPositionIndicator(FindClosestEnemyPositionToCamera());  
    }

    private void SetEnemyWaveNumberText(int waveNumber)
    {
        _enemyWaveNumberText.gameObject.SetActive(true);
        _enemyWaveNumberText.text = $"Wave: {waveNumber}";
    }

    private void SetEnemyWaveMessageText(int remainingTime)
    {
        _enemyWaveMessageText.gameObject.SetActive(true);
        _enemyWaveMessageText.text = $"New wave is coming in {remainingTime} seconds!";
    }

    private void SetNextWaveSpawnPointIndicator(Vector3 spawnPosition)
    {
        StartCoroutine(SetNextWaveSpawnPointIndicatorRoutine(spawnPosition));
    }

    private IEnumerator SetNextWaveSpawnPointIndicatorRoutine(Vector2 spawnPosition)
    {
        _enemySpawnPositionIndicator.gameObject.SetActive(true);

        Vector2 vectorDirection = (spawnPosition - (Vector2)_mainCamera.transform.position).normalized;
        
        _enemySpawnPositionIndicator.eulerAngles =
            new Vector3(0, 0, UtilsClass.GetAngleFromVector(vectorDirection));
        _enemySpawnPositionIndicator.anchoredPosition = vectorDirection * 150f;
        
        yield return new WaitForSeconds(2f);
        
        _enemySpawnPositionIndicator.gameObject.SetActive(false);
    }

    private void SetClosestEnemyPositionIndicator(Vector2 enemyPosition)
    {
        Vector2 vectorDirection = (enemyPosition - (Vector2)_mainCamera.transform.position).normalized;

        _enemyClosestPositionIndicator.eulerAngles = 
            new Vector3(0, 0, UtilsClass.GetAngleFromVector(vectorDirection));
        _enemyClosestPositionIndicator.anchoredPosition = vectorDirection * 100f;

        float distanceToClosestEnemy = Vector2.Distance(enemyPosition, _mainCamera.transform.position);
        _enemyClosestPositionIndicator.gameObject.
            SetActive(distanceToClosestEnemy > _mainCamera.orthographicSize * 1.5f);
    }

    private Vector2 FindClosestEnemyPositionToCamera()
    {
        Enemy closestEnemy = null;
        var cameraPosition = _mainCamera.transform.position;
        
        float targetMaxRadius = 9999;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(cameraPosition, targetMaxRadius);

        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                // It's an enemy

                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                else
                {
                    // Compare enemy distance with closest enemy distance
                    
                    float distance = Vector2.Distance(enemy.transform.position, cameraPosition);
                    float closestEnemyDistance =
                        Vector2.Distance(closestEnemy.transform.position, cameraPosition);

                    if (distance < closestEnemyDistance)
                    {
                        closestEnemy = enemy;
                    }
                }
            }
        }

        if (closestEnemy == null)
        {
            _enemyClosestPositionIndicator.gameObject.SetActive(false);
            return Vector2.zero;
        }

        return closestEnemy.transform.position;
    }

    private void HideEnemyWaveUI()
    {
        _enemyWaveNumberText.gameObject.SetActive(false);
        _enemyWaveMessageText.gameObject.SetActive(false);
    }
}
