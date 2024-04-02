using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
   private int _maxHealthAmount;
   private int _healthAmount;

   public event Action<bool> OnDamage;
   public event Action OnDie;
   public event Action OnGameOver;
   public event Action<bool> OnHeal;
   
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
      if (_healthAmount <= 0)
      {
         if (gameObject.CompareTag("HQ"))
         {
            // Game Over

            GameOverUI gameOverUI = FindAnyObjectByType<GameOverUI>(FindObjectsInactive.Include);
            
            gameOverUI.ShowGameOverUI();
            
            OnGameOver?.Invoke();
         }
         
         OnDie?.Invoke();
      }
      else
      {
         _healthAmount -= damageAmount; 
         OnDamage?.Invoke(false);
      }
   }
   
   public void Heal()
   {
      _healthAmount = _maxHealthAmount;
      OnHeal?.Invoke(true);
   }

   public float GetHealthAmountNormalized()
   {
      return (float)_healthAmount / _maxHealthAmount;
   }

   
}
