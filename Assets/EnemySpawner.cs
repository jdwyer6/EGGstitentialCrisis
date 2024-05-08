using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public int maxEnemies = 10; // Maximum number of enemies to spawn
    public float spawnRadius = 20f; // Radius within which enemies will be spawned

    void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            // Generate a random position within the specified radius
            Vector3 spawnPosition = Random.insideUnitCircle * spawnRadius;
            spawnPosition.z = spawnPosition.y;
            spawnPosition.y = 0; // Assuming you want to spawn on a flat surface

            // Instantiate the enemy at the generated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
