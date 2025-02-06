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

    void Update()
    {
        float gameTime = Time.timeSinceLevelLoad;
        if (gameTime < 10)
        {
            spawnRate = 2;
        }
        else if (gameTime < 20)
        {
            spawnRate = 1;
        }
        else if (gameTime < 30)
        {
            spawnRate = 0.5f;
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
