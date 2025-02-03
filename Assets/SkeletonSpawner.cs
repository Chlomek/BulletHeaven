using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public float spawnRate = 1;
    public float distanceFactor = 20;
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
            Vector3 randomPosition = Random.onUnitSphere * distanceFactor;
            GameObject newSkeleton = Instantiate(skeletonPrefab, transform.position + randomPosition, Quaternion.identity) as GameObject;
            lastSpawn = Time.timeSinceLevelLoad;
        }
    }
}
