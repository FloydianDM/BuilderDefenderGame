using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
   public static ResourceManager Instance { get; private set; }

   private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary = new();

   public event Action OnResourceAmountChanged;

   private void Awake()
   {
      ManageSingleton();
      
      ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>("ResourceTypes");

      foreach (var resourceType in resourceTypeList.ResourceTypes)
      {
         _resourceAmountDictionary[resourceType] = 0;
      }
   }

   private void ManageSingleton()
   {
      if (Instance != null)
      {
         gameObject.SetActive(false);
         Destroy(this);
      }
      else
      {
         Instance = this;
         DontDestroyOnLoad(this);
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
}
