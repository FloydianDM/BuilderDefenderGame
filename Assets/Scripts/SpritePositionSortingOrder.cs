using UnityEngine;

/// <summary>
/// To sort the sprites on map in correct order
/// </summary>
public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private float _positionOffsetY;
    
    private SpriteRenderer _spriteRenderer;
    private float _orderPrecisionMultiplier = 5f;
    private bool _isRunOnce;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _spriteRenderer.sortingOrder = (int)(-(transform.position.y + _positionOffsetY) * _orderPrecisionMultiplier);
        _isRunOnce = true;

        if (_isRunOnce)
        {
            Destroy(this);
        }
    }
}
