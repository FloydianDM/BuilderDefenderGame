using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeList")]
public class ResourceTypeListSO : ScriptableObject
{
    public List<ResourceTypeSO> ResourceTypes;
}
