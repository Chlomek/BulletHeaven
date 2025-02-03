using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    public int health;

    public GameObject orb;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (Random.Range(1, 10) > 3)
        {
            GameObject newOrb = Instantiate(orb, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
