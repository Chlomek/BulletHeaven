using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] public int damage = 20;
    [SerializeField] public float splashRadius = 3f;
    [SerializeField] private int rocketDespawn = 3;
    [SerializeField] private GameObject explosionEffect;
    public float speed = 7f;
    private Rigidbody2D rb;
    private Vector2 direction;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, rocketDespawn);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startSizeMultiplier = splashRadius / 7f;
            }
            Destroy(explosion, 2f);
        }
        ApplySplashDamage();
    }

    private void ApplySplashDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);
        HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

        foreach (Collider2D hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;
            GameObject rootObj = obj.transform.root.gameObject;

            if (rootObj.CompareTag("Player") || damagedObjects.Contains(rootObj))
                continue;

            damagedObjects.Add(rootObj);

            float distance = Vector2.Distance(rootObj.transform.position, transform.position);
            float damageMultiplier = Mathf.Clamp01(1.3f - (distance / splashRadius));
            int splashDamage = Mathf.RoundToInt(damage * damageMultiplier);
            splashDamage = Mathf.Max(damage / 3, splashDamage);

            if (rootObj.TryGetComponent<Health>(out Health entityHealth))
            {
                entityHealth.TakeDamage(splashDamage);
                Debug.Log($"{rootObj.name} took {splashDamage} splash damage");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}