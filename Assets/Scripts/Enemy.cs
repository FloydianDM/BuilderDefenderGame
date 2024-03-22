using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private Transform _targetTransform;
    private Rigidbody2D _enemyRigidbody;
    private float _moveSpeed = 5f;
    
    private void Start()
    {
        _buildingManager = FindFirstObjectByType<BuildingManager>();
        _targetTransform = _buildingManager.GetHqBuilding().transform; // Set target to HQ before finding any potential
        
        LookForTargets();
        _enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }
    
    private void LookForTargets()
    {
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
                
            return;
        }
        
        // Select random target

        int randomTargetIndex = Random.Range(0, potentialTargetList.Count);
        Collider2D randomTarget = potentialTargetList[randomTargetIndex];

        _targetTransform = randomTarget.GetComponent<Transform>();
    }

    private void MoveEnemy()
    {
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
        Destroy(gameObject);
    }

    public static Enemy CreateEnemy(Vector2 spawnPosition)
    {
        Transform enemyPrefab = Resources.Load<Transform>("EnemyPrefab");
        Transform enemyTransform = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        return enemyTransform.GetComponent<Enemy>();
    }
}
