using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private HealthSystem _healthSystem;
    
    private void Start()
    {
        HandleHealthBarUpdate(true);
        HideHealthBar(true);
        
        _healthSystem.OnDamage += HandleHealthBarUpdate;
        _healthSystem.OnHeal += HandleHealthBarUpdate;
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
