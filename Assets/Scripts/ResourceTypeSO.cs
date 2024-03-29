using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
public class ResourceTypeSO : ScriptableObject
{
    public string NameString;
    public string NameShort;
    public Sprite Sprite;
    public Color ResourceColor;
}
