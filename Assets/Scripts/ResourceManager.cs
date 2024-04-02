using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
   [SerializeField] private List<ResourceAmount> _startingResourceAmountList;

   private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary = new();

   public event Action OnResourceAmountChanged;

   private void Awake()
   {
      ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>("ResourceTypes");

      foreach (var resourceType in resourceTypeList.ResourceTypes)
      {
         _resourceAmountDictionary[resourceType] = 0;
      }

      foreach (ResourceAmount resourceAmount in _startingResourceAmountList)
      {
         AddResource(resourceAmount.ResourceType, resourceAmount.Amount);
      }
   }

   public void AddResource(ResourceTypeSO resourceType, int amount)
   {
      _resourceAmountDictionary[resourceType] += amount;
      OnResourceAmountChanged?.Invoke();
   }

   public int GetResourceAmount(ResourceTypeSO resourceType)
   {
      return _resourceAmountDictionary[resourceType];
   }

   public bool CanAfford(ResourceAmount[] resourceAmountArray)
   {
      foreach (var resourceAmount in resourceAmountArray)
      {
         if (GetResourceAmount(resourceAmount.ResourceType) < resourceAmount.Amount)
         {
            return false;
         }
      }

      return true;
   }

   public void SpendResources(ResourceAmount[] resourceAmountArray)
   {
      foreach (var resourceAmount in resourceAmountArray)
      {
         if (GetResourceAmount(resourceAmount.ResourceType) >= resourceAmount.Amount)
         {
            _resourceAmountDictionary[resourceAmount.ResourceType] -= resourceAmount.Amount;
         }
      }
   }
}
