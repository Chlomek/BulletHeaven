using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    public int health;
    [SerializeField]
    public int enemyValue = 1;
    private float lastHitTime = 0;

    private bool Isplayer = false;

    public GameObject orb;
    public GameObject DeathScreen;

    void Start()
    {
        health = maxHealth;
        Isplayer = IsPlayerCheck(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if(Isplayer)
        {
            if(Time.time - lastHitTime < 0.5)
            {
                return;
            }
            lastHitTime = Time.time;
        }

        health -= damage;    
        if (health <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (Isplayer)
        {
            Debug.Log("Player died");
            DeathScreen.GetComponent<DeathScreenManager>().ShowDeathScreen();
        }
        else
        {
            if (enemyValue >= 10)
            {
                if (Random.Range(1, 100) > 10)
                {
                    GameObject newOrb = Instantiate(orb, transform.position, Quaternion.identity);
                }
            }
            else if (enemyValue >= 5)
            { 
                if (Random.Range(1, 100) > 33)
                {
                    GameObject newOrb = Instantiate(orb, transform.position, Quaternion.identity);
                }
            }
            else
            {
                if (Random.Range(1, 100) > 50)
                {
                    GameObject newOrb = Instantiate(orb, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }

    public static bool IsPlayerCheck(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
}
