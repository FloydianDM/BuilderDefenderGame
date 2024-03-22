using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private HealthSystem _healthSystem;
    
    private void Start()
    {
        _healthSystem.OnDamage += HandleHealthBarUpdate;
        HandleHealthBarUpdate();
        HideHealthBar(_healthSystem.GetHealthAmountNormalized() == 1);
    }
    
    private void HandleHealthBarUpdate()
    {
        float barValue = _healthSystem.GetHealthAmountNormalized();
        _healthBar.fillAmount = barValue;
        HideHealthBar(_healthSystem.GetHealthAmountNormalized() == 1);
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
