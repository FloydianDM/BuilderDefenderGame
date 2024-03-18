using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string NameString;
    public Transform Prefab;
    public ResourceGeneratorData ResourceGeneratorData;
    public Sprite Sprite;
}
