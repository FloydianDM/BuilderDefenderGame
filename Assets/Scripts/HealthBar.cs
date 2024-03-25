using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private HealthSystem _healthSystem;
    
    private void Start()
    {
        HandleHealthBarUpdate();
        HideHealthBar(true);
        _healthSystem.OnDamage += HandleHealthBarUpdate;
    }
    
    private void HandleHealthBarUpdate()
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
