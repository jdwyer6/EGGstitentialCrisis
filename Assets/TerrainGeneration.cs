using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public List<Plane> planes;  
    public int gridWidth = 10; 
    public float spacing = 50f; 

    void Start()
    {
        spawnPlanes();
    }


    private void spawnPlanes() 
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                Plane plane = planes[Random.Range(0, planes.Count)];
                if (plane.prefab != null)
                {
                    Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                    Instantiate(plane.prefab, position, Quaternion.identity);
                }
            }
        }
    }

}
