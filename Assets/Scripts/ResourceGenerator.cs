using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
   private BuildingTypeSO _buildingType;
   private float _timer;
   private float _timerMax;

   private void Awake()
   {
      _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
      _timerMax = _buildingType.ResourceGeneratorData.TimerMax;
   }

   private void Update()
   {
      _timer += Time.deltaTime;

      if (_timer >= _timerMax)
      {
         _timer = 0;
         
         // add resource
         ResourceManager.Instance.AddResource(_buildingType.ResourceGeneratorData.ResourceType, 1);
      }
   }
}
