using UnityEngine;

[CreateAssetMenu(fileName = "WorldMaterial", menuName = "Assets/Create/WorldMaterial", order = 0)]
public class WorldMaterial : ScriptableObject
{
    public string materialName;
    public Sprite icon;
    public Material material;
}
