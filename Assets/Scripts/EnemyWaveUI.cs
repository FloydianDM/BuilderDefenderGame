using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemyWaveNumberText;
    [SerializeField] private TextMeshProUGUI _enemyWaveMessageText;
    [SerializeField] private RectTransform _enemySpawnPositionIndicator;

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

    private void HideEnemyWaveUI()
    {
        _enemyWaveNumberText.gameObject.SetActive(false);
        _enemyWaveMessageText.gameObject.SetActive(false);
    }
}
