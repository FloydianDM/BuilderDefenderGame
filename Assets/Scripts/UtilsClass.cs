using System.Collections;
using System.Collections.Generic;
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
}
