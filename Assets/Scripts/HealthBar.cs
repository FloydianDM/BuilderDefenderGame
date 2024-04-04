using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private Transform _separatorContainer;
    [SerializeField] private Transform _separatorTemplate;
    
    private void Start()
    {
        HandleHealthBarUpdate(true);
        HideHealthBar(true);
        SetSeparators();

        _healthSystem.OnDamage += HandleHealthBarUpdate;
        _healthSystem.OnHeal += HandleHealthBarUpdate;
        _healthSystem.OnHealthAmountMaxChanged += SetSeparators;
    }

    private void SetSeparators()
    {
        _separatorTemplate.gameObject.SetActive(false);

        foreach (Transform separator in _separatorContainer)
        {
            if (separator == _separatorTemplate)
            {
                continue;
            }
            
            Destroy(separator.gameObject);
        }

        float healthAmountPerSeparator = 10;
        float healthBarSize = _healthBar.rectTransform.rect.width;
        float separatorPositionDistance = healthBarSize / _healthSystem.MaxHealthAmount;
        
        int separatorCount = Mathf.FloorToInt(_healthSystem.MaxHealthAmount / healthAmountPerSeparator);

        for (int i = 1; i < separatorCount; i++)
        {
            Transform separatorTransform = Instantiate(_separatorTemplate, _separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(
                separatorPositionDistance * i * healthAmountPerSeparator, 0);
        }
    }

    private void HandleHealthBarUpdate(bool _)
    {
        float barValue = _healthSystem.GetHealthAmountNormalized();
        _healthBar.fillAmount = barValue;
        HideHealthBar((int)_healthSystem.GetHealthAmountNormalized() == 1);
    }

    private void HideHealthBar(bool isFullHealth)
    {
        if (isFullHealth)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
