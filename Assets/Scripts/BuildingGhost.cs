using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
   [SerializeField] private BuildingManager _buildingManager;
   [SerializeField] private GameObject _spriteGameObject;
   [SerializeField] private ResourceNearbyOverlay _resourceNearbyOverlay;
   
   private void Start()
   {
      HideGhost();
      _buildingManager.OnActiveBuildingChanged += HandleGhostSpriteChanged;
   }

   private void Update()
   {
      transform.position = UtilsClass.GetMouseWorldPosition();
   }

   private void HandleGhostSpriteChanged(BuildingTypeSO buildingType)
   {
      if (buildingType == null)
      {
         HideGhost();
      }
      else
      {
         ShowGhost(buildingType.Sprite);
         _resourceNearbyOverlay.ShowOverlay(buildingType.ResourceGeneratorData);
      }
   }

   private void ShowGhost(Sprite ghostSprite)
   {
      _spriteGameObject.SetActive(true);
      _spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
   }

   private void HideGhost()
   {
      _spriteGameObject.SetActive(false);
      _resourceNearbyOverlay.HideOverlay();
   }
}
