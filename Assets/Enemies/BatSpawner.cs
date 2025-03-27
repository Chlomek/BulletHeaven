using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public float spawnRate = 20f;
    public float minDistance = 15f;
    public float maxDistance = 20f;
    public int minBats = 20;
    public int maxBats = 40;
    public GameObject batPrefab;

    public float startSpawning = 30;
    public float endSpawning = 3000;

    private float lastSpawnTime;

    void Update()
    {
        float gameTime = Time.timeSinceLevelLoad;
        if (gameTime < startSpawning || gameTime > endSpawning)
        {
            return;
        }

        if (Time.timeSinceLevelLoad - lastSpawnTime >= spawnRate)
        {
            int numBatsToSpawn = Random.Range(minBats, maxBats + 1);

            float spawnDistance = Mathf.Lerp(minDistance, maxDistance, Mathf.Sqrt(Random.value));
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnDistance;
            Vector2 randomPosition = new Vector3(randomCircle.x, randomCircle.y) + transform.position;

            for (int i = 0; i < numBatsToSpawn; i++)
            {
                
                Vector2 spawnOffsetCircle = Random.insideUnitCircle.normalized;
                Vector2 spawnOffsetPosition = new Vector2(spawnOffsetCircle.x, spawnOffsetCircle.y);
                Vector2 spawnPosition = spawnOffsetPosition + randomPosition;
                Instantiate(batPrefab, spawnPosition, Quaternion.identity);
            }

            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }
}
