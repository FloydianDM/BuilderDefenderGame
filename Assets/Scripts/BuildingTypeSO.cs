using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string NameString;
    public Transform Prefab;
    public ResourceGeneratorData ResourceGeneratorData;
    public Sprite Sprite;
    public float MinConstructionRadius;
    public ResourceAmount[] ConstructionResourceCostArray;

    public string GetConstructionResourceCostString()
    {
        string str = "";

        foreach (var resourceAmount in ConstructionResourceCostArray)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(resourceAmount.ResourceType.ResourceColor); 
            
            str += "<color=#" + colorHex + ">" + 
                   resourceAmount.ResourceType.NameShort + resourceAmount.Amount + "</color>";
            str += " ";
        }

        return str;
    }
}
