using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _cameraMoveSpeed;
    
    private BuilderDefenderGameInputActions _inputActions;
    private float _defaultOrthographicSize;

    private void Start()
    {
        _inputActions = new BuilderDefenderGameInputActions();
        _inputActions.Player.Enable();

        _defaultOrthographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;

        _inputActions.Player.Zoom.performed += ZoomCamera;
        _inputActions.Player.DefaultCameraZoom.performed += RestoreZoom;
        
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector2 previousPosition = transform.position;
        Vector2 moveDirection = _inputActions.Player.Move.ReadValue<Vector2>().normalized;
        transform.position = (moveDirection * (_cameraMoveSpeed * Time.deltaTime)) + previousPosition;
    }
    
    private void ZoomCamera(InputAction.CallbackContext context)
    {
        float unitZoomValue = 2f;
        float currentOrthographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float zoomDirection = context.ReadValue<float>();
        
        if (zoomDirection < 0)
        {
            unitZoomValue = -unitZoomValue;
        }
        
        currentOrthographicSize += unitZoomValue;
        currentOrthographicSize = Mathf.Clamp(currentOrthographicSize, 8, 20);
        
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = currentOrthographicSize;
    }
    
    private void RestoreZoom(InputAction.CallbackContext context)
    {
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = _defaultOrthographicSize;
    }
}

