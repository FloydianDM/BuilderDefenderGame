using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;
    
    [SerializeField] private TextMeshProUGUI _tooltipText;
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private RectTransform _canvasRectTransform;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        ManageSingleton();
    }

    private void ManageSingleton()
    {
        if (Instance != null)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        HideTooltip();
    }

    private void Update()
    {
        FollowMousePosition();
    }

    private void FollowMousePosition()
    {
        Vector2 anchoredPosition = Mouse.current.position.ReadValue() / _canvasRectTransform.localScale;

        if (anchoredPosition.x + _backgroundRectTransform.rect.width > _canvasRectTransform.rect.width)
        {
            anchoredPosition.x = _canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + _backgroundRectTransform.rect.height > _canvasRectTransform.rect.height)
        {
            anchoredPosition.y = _canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
        }
        
        _rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string text)
    {
        _tooltipText.text = text;
        _tooltipText.ForceMeshUpdate(); // to ensure the update of component!
        
        Vector2 textSize = _tooltipText.GetRenderedValues(false);
        Vector2 padding = new Vector2(6, 6);
        _backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void ShowTooltipText(string text)
    {
        gameObject.SetActive(true);
        
        StartCoroutine(ShowTooltipTextRoutine(text));
    }

    private IEnumerator ShowTooltipTextRoutine(string text)
    {
        SetText(text);

        yield return new WaitForSeconds(2f);
        
        HideTooltip();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
