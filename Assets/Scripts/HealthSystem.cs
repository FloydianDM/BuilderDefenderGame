using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
   public int MaxHealthAmount { get; private set; }
   public int HealthAmount { get; private set; }

   public event Action<bool> OnDamage;
   public event Action OnDie;
   public event Action OnGameOver;
   public event Action<bool> OnHeal;
   public event Action OnHealthAmountMaxChanged;
   
   public void SetMaxHealthAmount(int maxHealthAmount, bool shouldUpdateHealthAmount)
   {
      MaxHealthAmount = maxHealthAmount;

      if (shouldUpdateHealthAmount)
      {
         HealthAmount = MaxHealthAmount;
      }

      OnHealthAmountMaxChanged?.Invoke();
   }

   public void TakeDamage(int damageAmount)
   {
      if (HealthAmount <= 0)
      {
         if (gameObject.CompareTag("HQ"))
         {
            // Game Over
            
            AudioManager.Instance.PlaySFX(AudioManager.SFX.GameOver);
            GameOverUI gameOverUI = FindAnyObjectByType<GameOverUI>(FindObjectsInactive.Include);
            gameOverUI.ShowGameOverUI();
            
            OnGameOver?.Invoke();
         }
         
         OnDie?.Invoke();
      }
      else
      {
         HealthAmount -= damageAmount; 
         OnDamage?.Invoke(false);
      }
   }
   
   public void Heal()
   {
      HealthAmount = MaxHealthAmount;
      OnHeal?.Invoke(true);
   }

   public float GetHealthAmountNormalized()
   {
      return (float)HealthAmount / MaxHealthAmount;
   }

   
}
