using System.Collections;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private float _spawnTimer = 10f;
    
    private void Start()
    {
        StartCoroutine(SpawnEnemyWave());
    }
    
    private IEnumerator SpawnEnemyWave()
    {
        while (true)
        {
            Vector2 spawnPosition = new Vector3(20, 10);
        
            for (int i = 0; i < 10; i++)
            {
                Enemy.CreateEnemy(spawnPosition + UtilsClass.GetRandomDirection() * Random.Range(0, 5));
            }

            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}
