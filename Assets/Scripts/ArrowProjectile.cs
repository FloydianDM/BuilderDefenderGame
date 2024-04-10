using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private Transform _arrowSprite;
    
    private Enemy _targetEnemy;
    private Vector3 _recordedMoveDirection;
    private float _moveSpeed = 20f;
    private float _destroyTimer = 3f;
    private int _damage = 5;
    
    private void Update()
    {
        MoveArrow();
        SetDestroyTimer();
    }

    private void SetTarget(Enemy targetEnemy)
    {
        _targetEnemy = targetEnemy;
    }

    private void MoveArrow()
    {
        if (_targetEnemy == null)
        {
            transform.position += _recordedMoveDirection * (_moveSpeed * Time.deltaTime);
        }
        else
        {
            Transform arrowTransform = transform;
            Transform enemyTransform = _targetEnemy.transform;
        
            Vector3 moveDirection = (enemyTransform.position - arrowTransform.position).normalized;
            _arrowSprite.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDirection));
            _recordedMoveDirection = moveDirection; // use this value if enemy destroyed while arrow was moving
        
            transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
        }
    }

    private void SetDestroyTimer()
    {
        _destroyTimer -= Time.deltaTime;

        if (_destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Tower tower))
        {
            return;
        }

        if (!other.TryGetComponent(out Enemy enemy))
        {
            Destroy(gameObject);
            
            return;
        }
        
        // Hit an enemy
            
        AudioManager.Instance.PlaySFX(AudioManager.SFX.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(8f, 0.8f);
        ChromaticAberrationEffect.Instance.SetChromaticAberrationWeight(0.2f);
        enemy.GetComponent<HealthSystem>().TakeDamage(_damage);
         
        Destroy(gameObject);
    }
    
    public static ArrowProjectile CreateArrowProjectile(GameObject arrow, Vector2 spawnPosition, Enemy enemy)
    {
        GameObject arrowProjectileTransform = Instantiate(arrow, spawnPosition, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowProjectileTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        
        return arrowProjectile;
    }
}
