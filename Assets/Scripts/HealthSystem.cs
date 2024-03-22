using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
   private int _maxHealthAmount;
   private int _healthAmount;

   public event Action OnDamage;
   public event Action OnDie;

   public void SetMaxHealthAmount(int maxHealthAmount, bool shouldUpdateHealthAmount)
   {
      _maxHealthAmount = maxHealthAmount;

      if (shouldUpdateHealthAmount)
      {
         _healthAmount = _maxHealthAmount;
      }
   }

   public void TakeDamage(int damageAmount)
   {
      _healthAmount -= damageAmount; 
      OnDamage?.Invoke();

      if (_healthAmount <= 0)
      {
         // Die
     
         OnDie?.Invoke();
      }
   }

   public float GetHealthAmountNormalized()
   {
      return (float)_healthAmount / _maxHealthAmount;
   }
}
