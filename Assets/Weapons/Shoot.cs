using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float firerate = 1;
    [SerializeField] private int Damage = 10;
    [SerializeField] private float bulletSpeed = 10f;

    public GameObject bullet;
    private GameObject myTarget;
    private float lastShot = 0.0f;
    private GameObject[] targets;
    private int lvl = 0;
    [SerializeField] private XPSystem xpSystem;

    //shoots at closest enemy

    void start()
    {
        GetPistolLevel();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }
    }

    public void ShootBullet()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
        Vector2 direction = (myTarget.transform.position - transform.position).normalized;

        ProjectileBehavior projectile = newBullet.GetComponent<ProjectileBehavior>();
        projectile.SetDirection(direction);
        projectile.speed = bulletSpeed;

        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        projectile.SetDamage(Damage);
    }

    void Update()
    {
        if (lvl != xpSystem.pistolLvl)
        {
            UpdatePistolLevel();
        }

        if (Time.timeSinceLevelLoad - lastShot >= firerate)
        {
            try
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
                myTarget = targets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
            }
            catch
            {
                return;
            }
            ShootBullet();
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    private void UpdatePistolLevel()
    {
        GetPistolLevel();

        switch (lvl)
        {
            case 1:
                firerate = 1;
                Damage = 10;
                bulletSpeed = 10f;
                break;
            case 2:
                firerate = 0.8f;
                Damage = 10;
                bulletSpeed = 10f;
                break;
            case 3:
                firerate = 0.8f;
                Damage = 12;
                bulletSpeed = 10f;
                break;
            case 4:
                firerate = 0.60f;
                Damage = 12;
                bulletSpeed = 10f;
                break;
            case 5:
                firerate = 0.6f;
                Damage = 12;
                bulletSpeed = 12;
                break;
            case 6:
                firerate = 0.2f;
                Damage = 6;
                bulletSpeed = 15;
                break;
            default:
                firerate = 1f / lvl;
                break;
        }
    }
    private void GetPistolLevel()
    {
        lvl = xpSystem.pistolLvl;
    }
}

