using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform _arrowSpawnTransform;
    
    private Enemy _targetEnemy;
    private float _detectionRate = 1f;
    private float _shootingRate = 0.5f;

    private void Start()
    {
        StartCoroutine(LookForTargets());
        StartCoroutine(HandleShooting());
    }

    private IEnumerator LookForTargets()
    {
        while (true)
        {
            float maxTargetRadius = 20f; // tower target range

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, maxTargetRadius);

            if (colliderArray.Length == 0)
            {
                // Don't look for the targets

                yield return new WaitForSeconds(_detectionRate);
            }
            else
            {
                foreach (Collider2D col in colliderArray)
                {
                    if (col.TryGetComponent(out Enemy enemy))
                    {
                        // It's an enemy

                        if (_targetEnemy == null)
                        {
                            _targetEnemy = enemy;
                        }
                        else
                        {
                            if (Vector3.Distance(transform.position, enemy.transform.position) <
                                Vector3.Distance(transform.position, _targetEnemy.transform.position))
                            {
                                _targetEnemy = enemy;
                            };
                        }
                    }
                }

                yield return new WaitForSeconds(_detectionRate);
            }
        }
    }

    private IEnumerator HandleShooting()
    {
        while (true)
        {
            if (_targetEnemy == null)
            {
                yield return new WaitForSeconds(_shootingRate);
            }
            else
            {
                ArrowProjectile.CreateArrowProjectile(_arrowSpawnTransform.position, _targetEnemy);
                
                yield return new WaitForSeconds(_shootingRate);
            }
        }
    }
}
