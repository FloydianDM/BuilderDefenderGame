using System;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
   private ResourceGeneratorData _resourceGeneratorData;
   private float _timer;
   private float _timerMax;
   private int _maxResourceAmount;

   public event Action OnGenerationRateChanged;
   public event Action OnTimeChanged;

   private void Awake()
   {
      _resourceGeneratorData = GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
      _timerMax = _resourceGeneratorData.TimerMax;
      _maxResourceAmount = _resourceGeneratorData.MaxResourceAmount;
   }

   private void Start()
   {
      int nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);

      if (nearbyResourceAmount == 0)
      {
         // disable the script, no resource nearby
         enabled = false;
      }
      else
      {
         // algorithm for tweaking collection speed regarding to nearby resource amount
         _timerMax = (_timerMax / 2f) + _timerMax * (1 - (float)nearbyResourceAmount / _maxResourceAmount);
         OnGenerationRateChanged?.Invoke();
      }
   }

   public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
   {
      Collider2D[] resourceColliderArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.ResourceCollectionRadius);
      
      int nearbyResourceAmount = 0;
      
      foreach (Collider2D resourceCollider in resourceColliderArray)
      {
         ResourceNode resourceNode = resourceCollider.GetComponent<ResourceNode>();
         
         if (resourceNode != null)
         {
            if (resourceNode.ResourceType == resourceGeneratorData.ResourceType)
            {
               nearbyResourceAmount++;
            }
         }
      }

      nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.MaxResourceAmount);
      return nearbyResourceAmount;
   }

   private void Update()
   {
      ExecuteTimer();
   }

   private void ExecuteTimer()
   {
      _timer += Time.deltaTime;
      
      OnTimeChanged?.Invoke();

      if (_timer >= _timerMax)
      {
         _timer = 0;
         
         // add resource
         ResourceManager.Instance.AddResource(_resourceGeneratorData.ResourceType, 1);
      }
   }

   public ResourceGeneratorData GetResourceGeneratorData()
   {
      return _resourceGeneratorData;
   }

   public float GetTimerNormalized()
   {
      return _timer / _timerMax;
   }

   public float GetGeneratedResourceAmountPerSecond()
   {
      return 1 / _timerMax;
   }
}
