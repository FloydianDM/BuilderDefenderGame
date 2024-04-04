using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _cameraMoveSpeed;
    
    private OptionsUI _optionsUI;
    private BuilderDefenderGameInputActions _inputActions;
    private float _defaultOrthographicSize;
    private bool _isEdgeScrollingOn = true;
    private float _edgeScrollingSize = 20f;

    private void Start()
    {
        _optionsUI = FindAnyObjectByType<OptionsUI>(FindObjectsInactive.Include);
        
        _inputActions = new BuilderDefenderGameInputActions();
        _inputActions.Player.Enable();

        _defaultOrthographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;

        _inputActions.Player.Zoom.performed += ZoomCamera;
        _inputActions.Player.DefaultCameraZoom.performed += RestoreZoom;

        _optionsUI.OnEdgeScrollingToggleChanged += ToggleEdgeScrolling;

    }
    
    private void Update()
    {
        MoveCamera();
    }
    
    private void ToggleEdgeScrolling()
    {
        _isEdgeScrollingOn = !_isEdgeScrollingOn;
    }
    
    private void MoveCamera()
    {
        Vector2 previousPosition = transform.position;
        Vector2 moveDirection = _inputActions.Player.Move.ReadValue<Vector2>().normalized;

        if (_isEdgeScrollingOn)
        {
            if (Mouse.current.position.ReadValue().x > Screen.width - _edgeScrollingSize)
            {
                moveDirection.x = 1f;
            }
            else if (Mouse.current.position.ReadValue().x < _edgeScrollingSize)
            {
                moveDirection.x = -1f;
            }

            if (Mouse.current.position.ReadValue().y > Screen.height - _edgeScrollingSize)
            {
                moveDirection.y = 1f;
            }
            else if (Mouse.current.position.ReadValue().y < _edgeScrollingSize)
            {
                moveDirection.y = -1f;
            }
        }
        
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

