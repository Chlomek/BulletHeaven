using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] private int damage = 20; // Higher damage for rockets
    [SerializeField] public float splashRadius = 3f; // Public so RocketLauncher can access it
    [SerializeField] private GameObject explosionEffect; // Prefab for the explosion visual effect
    public float speed = 7f; // Speed of the rocket

    private Rigidbody2D rb;
    private Vector2 direction;

    [SerializeField] private int rocketDespawn = 3;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Only set velocity if direction isn't already set by RocketLauncher
        if (rb.velocity.magnitude < 0.1f && direction != Vector2.zero)
        {
            rb.velocity = direction * speed;
        }

        Destroy(gameObject, rocketDespawn);

        // Debug log to verify rocket is moving
        Debug.Log("Rocket started with velocity: " + rb.velocity + " and direction: " + direction);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;

        // Set velocity immediately if RigidBody already exists
        if (rb != null)
        {
            rb.velocity = direction * speed;
            Debug.Log("Setting rocket velocity in SetDirection: " + rb.velocity);
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug when collision occurs
        Debug.Log("Rocket collided with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

        // Check if collided with an enemy or environment
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            // Create explosion and apply splash damage
            Explode();

            // Destroy the rocket
            Destroy(gameObject);
        }
    }

    // Separate method to ensure explosion happens in one place
    private void Explode()
    {
        Debug.Log("Rocket exploding at position: " + transform.position);

        // Create explosion effect
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Scale the explosion effect based on splash radius
            explosion.transform.localScale = Vector3.one * (splashRadius / 2f);

            // Destroy the explosion effect after some time
            Destroy(explosion, 2f);
        }

        // Apply splash damage to all enemies within radius
        ApplySplashDamage();
    }

    private void ApplySplashDamage()
    {
        // Find all colliders within the splash radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);
        Debug.Log("Found " + hitColliders.Length + " objects in splash radius");

        HashSet<GameObject> damagedObjects = new HashSet<GameObject>(); // Prevent double-damage

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Skip if it's not an enemy or already damaged
            if (!hitCollider.CompareTag("Enemy") || damagedObjects.Contains(hitCollider.gameObject))
                continue;

            // Add to damaged objects set
            damagedObjects.Add(hitCollider.gameObject);

            // Calculate damage based on distance
            Vector2 distanceVector = hitCollider.transform.position - transform.position;
            float distance = distanceVector.magnitude;

            // Reduce damage based on distance from impact point
            float damageMultiplier = 1 - (distance / splashRadius);
            int splashDamage = Mathf.RoundToInt(damage * damageMultiplier);

            // Ensure minimum damage of 25% of base damage
            splashDamage = Mathf.Max(damage / 4, splashDamage);

            // Apply splash damage
            if (hitCollider.gameObject.TryGetComponent<Health>(out Health entityHealth))
            {
                entityHealth.TakeDamage(splashDamage);
                Debug.Log(hitCollider.gameObject.name + " took " + splashDamage + " splash damage");
            }
        }
    }

    // Visualize the splash radius in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}