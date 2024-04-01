using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
   private int _maxHealthAmount;
   private int _healthAmount;
   private GameOverUI _gameOverUI;

   public event Action OnDamage;
   public event Action OnDie;
   public event Action OnGameOver;

   private void Start()
   {
      _gameOverUI = FindFirstObjectByType<GameOverUI>();
   }

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
         OnDie?.Invoke();
         
         if (gameObject.CompareTag("HQ"))
         {
            // Game Over
            
            _gameOverUI.ShowGameOverUI();
            
            OnGameOver?.Invoke();
         }
      }
      else
      {
         _healthAmount -= damageAmount; 
         OnDamage?.Invoke();
      }
   }

   public float GetHealthAmountNormalized()
   {
      return (float)_healthAmount / _maxHealthAmount;
   }
}
