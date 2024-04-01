using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxEnemyHealthAmount;
    
    private BuildingManager _buildingManager;
    private HealthSystem _healthSystem;
    private Transform _targetTransform;
    private Rigidbody2D _enemyRigidbody;
    private float _moveSpeed = 5f;
    
    private void Start()
    {
        _buildingManager = FindFirstObjectByType<BuildingManager>();
        _healthSystem = GetComponent<HealthSystem>();
        
        // Set target to HQ before finding any potential if HQ exists

        _targetTransform = _buildingManager.GetHqBuilding().transform;
        
        _healthSystem.SetMaxHealthAmount(_maxEnemyHealthAmount, true);
        _healthSystem.OnDie += DestroyEnemy;
        
        LookForTargets();
        _enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }
    
    private void LookForTargets()
    {
        if (_buildingManager.GetHqBuilding().transform == null)
        {
            return;
        }
        
        float maxTargetRadius = 10f;

        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, maxTargetRadius);

        if (colliderArray.Length == 0)
        {
            // Don't look for the targets, attack to the HQ

            return;
        }
        
        List<Collider2D> potentialTargetList = new List<Collider2D>();

        foreach (Collider2D col in colliderArray)
        {
            if (col.TryGetComponent(out Building building))
            {
                // It's a building
                
                potentialTargetList.Add(col);
            }
        }

        if (potentialTargetList.Count == 0)
        {
            // Don't look for the targets, attack to the HQ

            if (_buildingManager.GetHqBuilding().transform != null)
            {
                _targetTransform = _buildingManager.GetHqBuilding().transform;
            }
                
            return;
        }
        
        // Select random target

        int randomTargetIndex = Random.Range(0, potentialTargetList.Count);
        Collider2D randomTarget = potentialTargetList[randomTargetIndex];

        _targetTransform = randomTarget.GetComponent<Transform>();
    }

    private void MoveEnemy()
    {
        if (_buildingManager.GetHqBuilding().transform == null)
        {
            return;
        }
        
        Vector2 moveDirection = (_targetTransform.position - transform.position).normalized;
        _enemyRigidbody.velocity = _moveSpeed * moveDirection;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out Building building))
        {
            return;
        }
        
        building.GetComponent<HealthSystem>().TakeDamage(10);
        
        DestroyEnemy();
    }
    
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public GameObject CreateEnemy(GameObject enemyPrefab, Vector2 spawnPosition)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        return enemy;
    }
}
