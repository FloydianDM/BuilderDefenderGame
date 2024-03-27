using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
   [SerializeField] private Image _constructionProgressImage;
   [SerializeField] private BuildingConstruction _buildingConstruction;
   
   private void Update()
   {
      SetConstructionProgressImage();
   }

   private void SetConstructionProgressImage()
   {
      _constructionProgressImage.fillAmount = _buildingConstruction.GetConstructionTimerRatio();
   }
   
}
