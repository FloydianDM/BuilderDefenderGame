using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _nextWaveSpawnPoint;
    [Header("Settings")]
    [SerializeField] private float _spawnWaveTimer = 10f;

    private SpawnState _spawnState;
    private BuildingManager _buildingManager;
    private Enemy _enemy;
    private float _enemySpawnTimer;
    private int _enemyCountInWave = 5;
    private int _remainingTimeForNextWave = 3;
    public int WaveNumber { get; private set; }

    public event Action<int> OnEnemyWaveNumberChanged;
    public event Action<int> OnEnemyNextWaveRemainingTimeChanged;
    public event Action<Vector3> OnEnemyWavePositionInitialised; 
    public event Action OnWaveFinished;
    
    private void Start()
    {
        _spawnState = SpawnState.Spawn;
        _enemy = _enemyPrefab.GetComponent<Enemy>();
        _buildingManager = FindFirstObjectByType<BuildingManager>();
        
        _buildingManager.OnHQBuildingDown += StopEnemySpawner;
        
        StartCoroutine(SpawnEnemyWave());
    }
    

    private IEnumerator SpawnEnemyWave()
    {
        while (_spawnState == SpawnState.Spawn)
        {
            OnEnemyWaveNumberChanged?.Invoke(WaveNumber + 1);
            
            int spawnIndex = Random.Range(0, _spawnPoints.Length); // Select random spawn point
            Transform spawnPoint = _spawnPoints[spawnIndex];
            
            _nextWaveSpawnPoint.position = spawnPoint.position; // for spawn point indicator
            OnEnemyWavePositionInitialised?.Invoke(_nextWaveSpawnPoint.position);
            
            for (int i = 0; i < _enemyCountInWave; i++)
            {
                StartCoroutine(SpawnEnemy(spawnPoint.position));
            }

            yield return new WaitForSeconds(_spawnWaveTimer - _remainingTimeForNextWave);

            for (int remainingTime = _remainingTimeForNextWave; remainingTime > 0; remainingTime--)
            {
                OnEnemyNextWaveRemainingTimeChanged?.Invoke(remainingTime);

                yield return new WaitForSeconds(1);
            }
            
            OnWaveFinished?.Invoke();
            WaveNumber++;
            _enemyCountInWave += WaveNumber;
        }
    }

    private IEnumerator SpawnEnemy(Vector2 spawnPosition)
    {
        _enemy.CreateEnemy(_enemyPrefab, 
            spawnPosition + UtilsClass.GetRandomDirection() * Random.Range(0, 10));
        
        _enemySpawnTimer = Random.Range(0, 0.2f);

        yield return new WaitForSeconds(_enemySpawnTimer);
    }
    
    private void StopEnemySpawner()
    {
        _spawnState = SpawnState.Wait;

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            enemy.DestroyEnemy();
        }
    }
    
    public enum SpawnState
    {
        Spawn,
        Wait
    }
}
