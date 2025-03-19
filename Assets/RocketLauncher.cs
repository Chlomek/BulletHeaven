using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private int Damage = 10;
    [SerializeField] private float firerate = 2; 
    [SerializeField] private float rocketSpeed = 7;
    [SerializeField] private float splashRadius = 3;
    public GameObject rocket;
    private GameObject myTarget;
    private float lastShot = 0.0f;
    private GameObject[] targets;
    private int lvl = 0;
    [SerializeField] private XPSystem xpSystem;

    void Start()
    {
        GetRPGLevel();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }
    }

    public void FireRocket()
    {
        GameObject newRocket = Instantiate(rocket, transform.position, transform.rotation);
        Vector2 direction = (myTarget.transform.position - transform.position).normalized;

        RocketBehavior projectile = newRocket.GetComponent<RocketBehavior>();
        projectile.SetDirection(direction);
        projectile.speed = rocketSpeed;
        projectile.damage = Damage;
        projectile.splashRadius = splashRadius;

        Rigidbody2D rb = newRocket.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * rocketSpeed;
        }
        projectile.SetDamage(Damage);
    }

    void Update()
    {
        if (lvl != xpSystem.rpgLvl)
        {
            UpdaterpgLevel();
        }

        if (Time.timeSinceLevelLoad - lastShot >= firerate)
        {
            try
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
                myTarget = GetBestSplashTarget();

                if (myTarget == null)
                {
                    myTarget = targets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
                }
            }
            catch
            {
                return;
            }

            FireRocket();
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    private GameObject GetBestSplashTarget()
    {
        GameObject bestTarget = null;
        int maxNearbyEnemies = 0;

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

    private void UpdaterpgLevel()
    {
        GetRPGLevel();

        switch (lvl)
        {
            case 0:
                gameObject.SetActive(false);
                Debug.Log("rpg weapon deactivated (level 0)");
                return;
            case 1:
                Damage = 15;
                firerate = 2f;
                rocketSpeed = 7;
                splashRadius = 2f;
                break;
            case 2:
                Damage = 18;
                firerate = 2f;
                rocketSpeed = 7;
                splashRadius = 2.4f;
                break;
            case 3:
                Damage = 18;
                firerate = 1.7f;
                rocketSpeed = 7;
                splashRadius = 2.4f;
                break;
            case 4:
                Damage = 18;
                firerate = 1.7f;
                rocketSpeed = 7;
                splashRadius = 2.4f;
                break;
            case 5:
                Damage = 18;
                firerate = 1.7f;
                rocketSpeed = 7;
                splashRadius = 3f;
                break; 
            default:
                Damage = 10;
                firerate = 2f;
                rocketSpeed = 7;
                splashRadius = 2f;
                break;
        }
    }

    private void GetRPGLevel()
    {
        lvl = xpSystem.rpgLvl;
    }
}