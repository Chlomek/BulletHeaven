using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public float spawnRate = 1;
    public float minDistance = 20;
    public float maxDistance = 20;
    private float lastSpawn;
    public GameObject skeletonPrefab;
    public float startSpawning = 0;
    public float endSpawning = 60;

    public float firstSpawnRate = 1;
    public float secondSpawnRate = 0.7f;
    public float thirdSpawnRate = 0.4f;

    void Update()
    {
        float gameTime = Time.timeSinceLevelLoad;
        if (gameTime < startSpawning || gameTime > endSpawning)
        {
            return;
        }
        if (gameTime < endSpawning/3)
        {
            spawnRate = firstSpawnRate;
        }
        else if (gameTime < endSpawning / 3 * 2)
        {
            spawnRate = secondSpawnRate;
        }
        else if (gameTime < endSpawning)
        {
            spawnRate = thirdSpawnRate;
        }
        else
        {
            spawnRate = 0.25f;
        }

        if (Time.timeSinceLevelLoad - lastSpawn >= spawnRate)
        {
            /*
            Vector3 randomPosition = Random.onUnitSphere * distanceFactor;
            GameObject newSkeleton = Instantiate(skeletonPrefab, transform.position + randomPosition, Quaternion.identity) as GameObject;
            */
            float spawnDistance = Mathf.Lerp(minDistance, maxDistance, Mathf.Sqrt(Random.value));
            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 randomPosition = new Vector3(randomCircle.x, randomCircle.y, 0);

            GameObject newSkeleton = Instantiate(skeletonPrefab, transform.position + randomPosition, Quaternion.identity);

            lastSpawn = Time.timeSinceLevelLoad;
        }
    }
}
