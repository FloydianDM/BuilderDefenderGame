using UnityEngine;
using UnityEngine.InputSystem;

public static class UtilsClass
{
    private static Camera _mainCamera;
    
    public static Vector2 GetMouseWorldPosition()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        
        Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return mouseWorldPosition;
    }

    public static Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    public static float GetAngleFromVector(Vector2 vector)
    {
        float vectorAngleInRad = Mathf.Atan2(vector.y, vector.x);
        float vectorAngleInDeg = vectorAngleInRad * Mathf.Rad2Deg;
        
        return vectorAngleInDeg;
    }
}
