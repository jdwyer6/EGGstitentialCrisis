using UnityEngine;

[CreateAssetMenu(fileName = "Plane", menuName = "Assets/Plane", order = 1)]
public class Plane : ScriptableObject
{
    public GameObject prefab;
    public bool[] occupiedConnectionPoints = new bool[4] { false, false, false, false };
    public int assignedGridNumber = 0;
}
