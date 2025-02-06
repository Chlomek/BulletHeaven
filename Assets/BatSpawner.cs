using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public float spawnRate = 20f;  // Time in seconds between spawns (every 20 seconds)
    public float minDistance = 15f; // Minimum distance from the spawner to spawn bats
    public float maxDistance = 20f; // Maximum distance from the spawner to spawn bats
    public int minBats = 20;       // Minimum number of bats in a group
    public int maxBats = 40;       // Maximum number of bats in a group
    public GameObject batPrefab;   // Bat prefab to spawn

    private float lastSpawnTime;

    void Update()
    {
        if (Time.timeSinceLevelLoad - lastSpawnTime >= spawnRate)
        {
            // Spawn a random number of bats between minBats and maxBats
            int numBatsToSpawn = Random.Range(minBats, maxBats + 1);

            float spawnDistance = Mathf.Lerp(minDistance, maxDistance, Mathf.Sqrt(Random.value));
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 randomPosition = new Vector3(randomCircle.x, randomCircle.y, 0);

            for (int i = 0; i < numBatsToSpawn; i++)
            {
                
                Vector2 spawnOffsetCircle = Random.insideUnitCircle.normalized;
                Vector3 spawnOffsetPosition = new Vector3(spawnOffsetCircle.x, spawnOffsetCircle.y, 0);
                Vector3 spawnPosition = spawnOffsetPosition + randomPosition;  // Offset by spawner's position
                // Instantiate the bat at the calculated position
                Instantiate(batPrefab, spawnPosition, Quaternion.identity);
            }

            // Update the last spawn time
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }
}
