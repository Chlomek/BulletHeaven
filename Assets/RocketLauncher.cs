using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketLauncher : MonoBehaviour
{
    public float firerate = 2; // Slower fire rate for rocket launcher
    public GameObject rocket; // Rocket prefab
    private GameObject myTarget;
    private float lastShot = 0.0f;
    private GameObject[] targets;
    public float rocketSpeed = 7; // Slower speed for rockets
    private int lvl;

    [SerializeField] private AudioSource launchSound; // Optional audio for firing
    [SerializeField] private Transform launchPoint; // Optional spawn point for rockets

    void Start()
    {
        GetRPGLevel();

        // If no launch point is specified, use the current transform
        if (launchPoint == null)
            launchPoint = transform;
    }

    public void FireRocket()
    {
        // Use launch point position if available, otherwise use transform position
        Vector3 spawnPosition = launchPoint != null ? launchPoint.position : transform.position;

        GameObject newRocket = Instantiate(rocket, spawnPosition, transform.rotation);
        Vector2 direction = (myTarget.transform.position - transform.position).normalized;

        // First set the direction for the ProjectileBehavior
        RocketBehavior projectile = newRocket.GetComponent<RocketBehavior>();
        if (projectile != null)
        {
            projectile.SetDirection(direction);
            projectile.speed = rocketSpeed;
        }

        // Ensure the Rigidbody2D velocity is also set directly
        Rigidbody2D rb = newRocket.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * rocketSpeed;
        }

        // Play launch sound if available
        if (launchSound != null)
            launchSound.Play();
    }

    void Update()
    {
        GetRPGLevel();

        // Adjust fire rate based on RPG level
        switch (lvl)
        {
            case 1:
                firerate = 2f; // Slower base fire rate
                break;
            case 2:
                firerate = 1.8f;
                break;
            case 3:
                firerate = 1.6f;
                break;
            case 4:
                firerate = 1.4f;
                break;
            case 5:
                firerate = 1.2f;
                break;
            default:
                // For higher levels, continue to decrease fire rate but with diminishing returns
                firerate = 2f / (1 + (lvl - 1) * 0.2f);
                break;
        }

        if (Time.timeSinceLevelLoad - lastShot >= firerate)
        {
            try
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");

                // Target the enemy with the most nearby enemies for maximum splash damage
                myTarget = GetBestSplashTarget();

                // If no good splash target is found, target the closest enemy
                if (myTarget == null)
                {
                    myTarget = targets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
                }
            }
            catch
            {
                return; // No enemies found
            }

            FireRocket();
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    private GameObject GetBestSplashTarget()
    {
        GameObject bestTarget = null;
        int maxNearbyEnemies = 0;

        // Get splash radius from rocket prefab if possible
        float splashRadius = 3f;
        if (rocket.TryGetComponent<RocketBehavior>(out RocketBehavior rocketBehavior))
        {
            // This assumes you've made splashRadius public or created a getter method
            splashRadius = rocketBehavior.splashRadius;
        }

        foreach (GameObject potentialTarget in targets)
        {
            // Count how many enemies are within splash radius of this target
            int nearbyEnemies = 0;
            foreach (GameObject other in targets)
            {
                if (other != potentialTarget &&
                    Vector2.Distance(potentialTarget.transform.position, other.transform.position) <= splashRadius)
                {
                    nearbyEnemies++;
                }
            }

            // Find the target with the most nearby enemies
            if (nearbyEnemies > maxNearbyEnemies)
            {
                maxNearbyEnemies = nearbyEnemies;
                bestTarget = potentialTarget;
            }
        }

        // Return the best target if at least one other enemy is nearby, otherwise null
        return maxNearbyEnemies > 0 ? bestTarget : null;
    }

    private void GetRPGLevel()
    {
        // Updated to use rpgLvl instead of pistolLvl
        lvl = GameObject.Find("Player").GetComponent<XPSystem>().rpgLvl;
    }
}