using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
   private ResourceGeneratorData _resourceGeneratorData;
   private float _timer;
   private float _timerMax;
   private float _resourceCollectionRadius;
   private int _maxResourceAmount;

   private void Awake()
   {
      _resourceGeneratorData = GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
      _timerMax = _resourceGeneratorData.TimerMax;
      _resourceCollectionRadius = _resourceGeneratorData.ResourceCollectionRadius;
      _maxResourceAmount = _resourceGeneratorData.MaxResourceAmount;
   }

   private void Start()
   {
      Collider2D[] resourceColliderArray = Physics2D.OverlapCircleAll(transform.position, _resourceCollectionRadius);
      
      int nearbyResourceAmount = 0;
      
      foreach (Collider2D resourceCollider in resourceColliderArray)
      {
         ResourceNode resourceNode = resourceCollider.GetComponent<ResourceNode>();
         
         if (resourceNode != null)
         {
            if (resourceNode.ResourceType == _resourceGeneratorData.ResourceType)
            {
               nearbyResourceAmount++;
            }
         }
         else
         {
            // algorithm for tweaking collection speed regarding to nearby resource amount
            _timerMax = (_timerMax / 2f) + _timerMax * (1 - (float)nearbyResourceAmount / _maxResourceAmount);
         }
      }

      nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, _maxResourceAmount);

      if (nearbyResourceAmount == 0)
      {
         enabled = false; // disable the script
      }
   }

   private void Update()
   {
      ExecuteTimer();
   }

   private void ExecuteTimer()
   {
      _timer += Time.deltaTime;

      if (_timer >= _timerMax)
      {
         _timer = 0;
         
         // add resource
         ResourceManager.Instance.AddResource(_resourceGeneratorData.ResourceType, 1);
      }
   }
}
