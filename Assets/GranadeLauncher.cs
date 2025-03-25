using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float splashRadius = 3f;
    [SerializeField] private float targetingRange = 10f;
    public GameObject grenadePrefab;
    private float lastShot = 0.0f;
    private int lvl = 0;
    [SerializeField] private XPSystem xpSystem;

    void Start()
    {
        UpdateGrenadeLevel();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }
    }

    void Update()
    {
        if (lvl != xpSystem.granadeLvl)
        {
            UpdateGrenadeLevel();
        }

        if (Time.timeSinceLevelLoad - lastShot >= fireRate)
        {
            GameObject target = GetRandomTarget();
            if (target != null)
            {
                DropGrenade(target);
                lastShot = Time.timeSinceLevelLoad;
            }
        }
    }

    private GameObject GetRandomTarget()
    {
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
        if (enemiesInRange.Count == 0)  return null;
        return enemiesInRange[Random.Range(0, enemiesInRange.Count)];
    }

    private void DropGrenade(GameObject target)
    {
        Vector2 dropPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        GameObject newGrenade = Instantiate(grenadePrefab, dropPosition, Quaternion.identity);

        GrenadeBehavior grenade = newGrenade.GetComponent<GrenadeBehavior>();
        grenade.damage = damage;
        grenade.splashRadius = splashRadius;
    }

    private void UpdateGrenadeLevel()
    {
        GetGrenadeLevel();

        switch (lvl)
        {
            case 0:
                gameObject.SetActive(false);
                Debug.Log("Grenade launcher deactivated (level 0)");
                break;
            case 1:
                damage = 15;
                fireRate = 2f;
                splashRadius = 2f;
                break;
            case 2:
                damage = 18;
                fireRate = 1.8f;
                splashRadius = 2.5f;
                break;
            case 3:
                damage = 22;
                fireRate = 1.6f;
                splashRadius = 3f;
                break;
            case 4:
                damage = 25;
                fireRate = 1.5f;
                splashRadius = 3.5f;
                break;
            case 5:
                damage = 30;
                fireRate = 1.3f;
                splashRadius = 4f;
                break;
            default:
                damage = 10;
                fireRate = 2f;
                splashRadius = 2f;
                break;
        }
    }

    private void GetGrenadeLevel()
    {
        lvl = xpSystem.granadeLvl;
    }
}
