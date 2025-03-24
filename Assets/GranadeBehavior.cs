using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    [SerializeField] public int damage = 20;
    [SerializeField] public float splashRadius = 3f;
    [SerializeField] private float grenadeDespawnTime = 1.5f;
    [SerializeField] private GameObject explosionEffect;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(grenadeDespawnTime);
        Explode();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
