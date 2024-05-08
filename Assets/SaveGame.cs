using UnityEngine;
using System;
using System.Linq;

public class SaveGame : MonoBehaviour
{
    public GameObject sculptablePrefab; 

    public void SavePositions(int slot)
    {
        GameObject[] sculptables = GameObject.FindGameObjectsWithTag("Sculptable");
        Vector3[] positions = sculptables.Select(sculptable => new Vector3(sculptable.transform.position.x, sculptable.transform.position.y - 0.25f, sculptable.transform.position.z)).ToArray();
        // Remove " (Instance)" from the material name if it's there
        string[] materials = sculptables.Select(sculptable => 
            sculptable.GetComponent<Renderer>().material.name.Replace(" (Instance)", "")).ToArray();

        string positionsKey = $"cubePositions_{slot}";
        string positionsJson = JsonUtility.ToJson(new SerializableVector3Array(positions));
        PlayerPrefs.SetString(positionsKey, positionsJson);

        string materialsKey = $"cubeMaterials_{slot}";
        string materialsJson = JsonUtility.ToJson(new SerializableStringArray(materials));
        PlayerPrefs.SetString(materialsKey, materialsJson);

        PlayerPrefs.Save();
    }


    public void LoadPositions(int slot)
    {
        string positionsKey = $"cubePositions_{slot}";
        string positionsJson = PlayerPrefs.GetString(positionsKey, string.Empty);

        // Assuming you've saved material indices instead of names.
        string materialsKey = $"cubeMaterials_{slot}";
        string materialsJson = PlayerPrefs.GetString(materialsKey, string.Empty);

        if (!string.IsNullOrEmpty(positionsJson) && !string.IsNullOrEmpty(materialsJson))
        {
            SerializableVector3Array loadedPositionsData = JsonUtility.FromJson<SerializableVector3Array>(positionsJson);
            Vector3[] positions = loadedPositionsData.ToVector3Array();

            SerializableStringArray loadedMaterialsData = JsonUtility.FromJson<SerializableStringArray>(materialsJson);
            string[] materialNames = loadedMaterialsData.ToArray(); // These should ideally be the names or identifiers.

            // Accessing the Data component to get the WorldMaterial objects.
            WorldMaterial[] worldMaterials = GetComponent<Data>().materials;

            for (int i = 0; i < positions.Length; i++)
            {
                GameObject sculptable;
                if (i < GameObject.FindGameObjectsWithTag("Sculptable").Length)
                {
                    sculptable = GameObject.FindGameObjectsWithTag("Sculptable")[i];
                    sculptable.transform.position = positions[i];
                }
                else
                {
                    sculptable = Instantiate(sculptablePrefab, positions[i], Quaternion.identity);
                }

                // Find the WorldMaterial object by name. This assumes materialNames[i] matches the WorldMaterial.materialName.
                WorldMaterial worldMaterial = worldMaterials.FirstOrDefault(mat => mat.materialName == materialNames[i]);
                if (worldMaterial != null)
                {
                    sculptable.GetComponentInChildren<Renderer>().material = worldMaterial.material;
                }
                else
                {
                    Debug.LogWarning($"WorldMaterial named {materialNames[i]} not found.");
                }
            }
        }
    }

    public void SaveAndQuit(int slot) 
    {
        SavePositions(slot);
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }

    [Serializable]
    private class SerializableVector3Array
    {
        public SerializableVector3[] positions;

        public SerializableVector3Array(Vector3[] vector3Array)
        {
            positions = new SerializableVector3[vector3Array.Length];
            for (int i = 0; i < vector3Array.Length; i++)
            {
                positions[i] = vector3Array[i];
            }
        }

        public Vector3[] ToVector3Array()
        {
            Vector3[] vector3Array = new Vector3[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                vector3Array[i] = positions[i];
            }
            return vector3Array;
        }
    }

    [Serializable]
    private struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3 { x = rValue.x, y = rValue.y, z = rValue.z };
        }
    }

    [Serializable]
    public class SerializableStringArray
    {
        public string[] array;

        public SerializableStringArray(string[] array)
        {
            this.array = array;
        }

        public string[] ToArray()
        {
            return array;
        }
    }

}
