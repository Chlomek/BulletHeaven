using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : MonoBehaviour
{
    [SerializeField] private float firerate = 1.5f;
    [SerializeField] private int damagePerPellet = 5;
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private XPSystem xpSystem;

    private float lastShot = 0.0f;
    private int lvl = 0;
    public Movement movementScript;

    void Start()
    {
        GetShotgunLevel();
        if (movementScript == null)
        {
            Debug.LogError("Movement script not found");
        }

        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }
    }

    public void ShootShotgun()
    {
        // Get the direction the player is facing - assuming the parent object has the player movement script
        Vector2 playerDirection = GetPlayerLookDirection();

        // Fire multiple pellets with spread
        for (int i = 0; i < pelletCount; i++)
        {
            // Calculate spread angle for this pellet
            float angle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector2 direction = RotateVector(playerDirection, angle).normalized;

            // Create bullet
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            // Set bullet properties
            ProjectileBehavior projectile = newBullet.GetComponent<ProjectileBehavior>();
            if (projectile != null)
            {
                projectile.SetDirection(direction);
                projectile.speed = bulletSpeed;
                projectile.SetDamage(damagePerPellet);
            }

            // Apply velocity
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }
    }

    void Update()
    {
        if (lvl != xpSystem.shotgunLvl)
        {
            UpdateShotgunLevel();
        }

        // Check if it's time to shoot
        if (Time.timeSinceLevelLoad - lastShot >= firerate)
        {
            ShootShotgun();
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    private Vector2 lastLookDirection = Vector2.right;

    private Vector2 GetPlayerLookDirection()
    {
        return movementScript.lookDirection;
    }


    private Vector2 RotateVector(Vector2 vector, float angleDegrees)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    private void UpdateShotgunLevel()
    {
        GetShotgunLevel();
        switch (lvl)
        {
            case 1:
                firerate = 1.5f;
                damagePerPellet = 5;
                bulletSpeed = 12f;
                pelletCount = 6;
                spreadAngle = 30f;
                break;
            case 2:
                firerate = 1.4f;
                damagePerPellet = 5;
                bulletSpeed = 12f;
                pelletCount = 7;
                spreadAngle = 28f;
                break;
            case 3:
                firerate = 1.3f;
                damagePerPellet = 6;
                bulletSpeed = 12f;
                pelletCount = 7;
                spreadAngle = 26f;
                break;
            case 4:
                firerate = 1.2f;
                damagePerPellet = 6;
                bulletSpeed = 13f;
                pelletCount = 8;
                spreadAngle = 24f;
                break;
            case 5:
                firerate = 1.0f;
                damagePerPellet = 7;
                bulletSpeed = 13f;
                pelletCount = 8;
                spreadAngle = 22f;
                break;
            case 6:
                firerate = 0.8f;
                damagePerPellet = 8;
                bulletSpeed = 14f;
                pelletCount = 10;
                spreadAngle = 20f;
                break;
            default:
                // For levels beyond 6, continue improving
                firerate = Mathf.Max(0.3f, 1.5f - (lvl * 0.1f));
                pelletCount = 6 + lvl;
                break;
        }
    }

    private void GetShotgunLevel()
    {
        // Assuming you'll add a shotgunLvl property to your XPSystem class
        lvl = xpSystem.shotgunLvl;
    }
}