using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float laserRange = 10f;
    [SerializeField] private float laserWidth = 0.5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserDuration = 0.2f;
    [SerializeField] private XPSystem xpSystem;
    [SerializeField] private float targetingRange = 15f;

    private float lastShot = 0.0f;
    private int lvl = 0;
    private GameObject currentLaser;
    private LineRenderer lineRenderer;

    void Start()
    {
        GetLaserLevel();
        UpdateLaserLevel();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }
    }

    void Update()
    {
        if (lvl != xpSystem.laserLvl)
        {
            UpdateLaserLevel();
        }

        if (Time.timeSinceLevelLoad - lastShot >= fireRate)
        {
            FireLaser();
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    private void FireLaser()
    {
        // Find enemies within targeting range
        List<GameObject> enemiesInRange = new List<GameObject>();
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= targetingRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

        // If no enemies in range, don't fire
        if (enemiesInRange.Count == 0)
            return;

        // Select random enemy from those in range
        GameObject targetEnemy = enemiesInRange[Random.Range(0, enemiesInRange.Count)];

        // Calculate direction toward the selected enemy
        Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;

        // Create laser beam
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        LineRenderer laserLine = laser.GetComponent<LineRenderer>();

        if (laserLine != null)
        {
            // Set up line renderer
            laserLine.startWidth = laserWidth;
            laserLine.endWidth = laserWidth;
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, transform.position + (Vector3)(direction * laserRange));

            // Handle the damage along the laser path
            ApplyLaserDamage(transform.position, direction);

            // Destroy the laser after duration
            Destroy(laser, laserDuration);
        }
    }

    private void ApplyLaserDamage(Vector2 origin, Vector2 direction)
    {
        // Use RaycastAll to hit all enemies in the laser's path
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, laserRange);
        HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

        foreach (RaycastHit2D hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            GameObject rootObj = obj.transform.root.gameObject;

            // Skip player and already damaged objects
            if (rootObj.CompareTag("Player") || damagedObjects.Contains(rootObj))
                continue;

            damagedObjects.Add(rootObj);

            // Apply damage to the root object
            if (rootObj.TryGetComponent<Health>(out Health entityHealth))
            {
                entityHealth.TakeDamage(damage);
                Debug.Log($"{rootObj.name} took {damage} laser damage");
            }
        }
    }

    private void UpdateLaserLevel()
    {
        GetLaserLevel();
        switch (lvl)
        {
            case 0:
                gameObject.SetActive(false);
                Debug.Log("laser weapon deactivated (level 0)");
                break;
            case 1:
                fireRate = 2f;
                damage = 15;
                laserRange = 12f;
                laserWidth = 0.2f;
                targetingRange = 12f;
                break;
            case 2:
                fireRate = 1.8f;
                damage = 15;
                laserRange = 12f;
                laserWidth = 0.2f;
                targetingRange = 12f;
                break;
            case 3:
                fireRate = 1.6f;
                damage = 20;
                laserRange = 12f;
                laserWidth = 0.2f;
                targetingRange = 12f;
                break;
            case 4:
                fireRate = 1.6f;
                damage = 20;
                laserRange = 15f;
                laserWidth = 0.2f;
                targetingRange = 15f;
                break;
            case 5:
                fireRate = 1.4f;
                damage = 20;
                laserRange = 16f;
                laserWidth = 0.2f;
                targetingRange = 16f;
                break;
            case 6:
                fireRate = 1.2f;
                damage = 30;
                laserRange = 20f;
                laserWidth = 0.3f;
                targetingRange = 20f;
                break;
            default:
                fireRate = 1.2f / lvl;
                damage = 15 + (lvl * 3);
                laserRange = 10f + (lvl * 2);
                laserWidth = 0.5f + (lvl * 0.1f);
                targetingRange = 15f + (lvl * 1f);
                break;
        }
    }

    private void GetLaserLevel()
    {
        lvl = xpSystem.laserLvl;
    }
}