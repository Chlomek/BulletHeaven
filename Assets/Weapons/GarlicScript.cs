using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicScript : MonoBehaviour
{
    [SerializeField] private float damageRate = 1f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float range = 1.5f;
    [SerializeField] private int lvl = 0;

    private Dictionary<GameObject, float> enemyLastHitTime = new Dictionary<GameObject, float>();
    [SerializeField] private XPSystem xpSystem;
    private CircleCollider2D circleCollider;
    private ParticleSystem ps;

    void Start()
    {
        xpSystem = FindObjectOfType<XPSystem>();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }

        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogError("CircleCollider2D not found on the garlic weapon!");
        }
        ps = GetComponent<ParticleSystem>();
        if (ps == null)
        {
            Debug.LogError("ParticleSystem not found on the garlic weapon!");
        }

        UpdateGarlicLevel();
    }

    void Update()
    {
        if (lvl != xpSystem.garlicLvl)
        {
            UpdateGarlicLevel();
        }
    }

    private void UpdateGarlicLevel()
    {
        GetGarlicLevel();
        var main = ps.main;

        switch (lvl)
        {
            case 0:
                gameObject.SetActive(false);
                Debug.Log("Garlic weapon deactivated (level 0)");
                break;
            case 1:
                damage = 4;
                damageRate = 1.0f;
                range = 2.5f;
                main.startLifetime = 2f; 
                break;

            case 2:
                damage = 4;
                damageRate = 1.0f;
                range = 3f;
                main.startLifetime = 2.5f;
                break;
            case 3:
                damage = 7;
                damageRate = 1.0f;
                range = 3f;
                main.startLifetime = 2.5f;
                break;
            case 4:
                damage = 10;
                damageRate = 0.8f;
                range = 3f;
                break;
            case 5:
                damage = 15;
                damageRate = 0.8f;
                range = 3.5f;
                main.startLifetime = 3f;
                break;
            default:
                damage = 2;
                damageRate = 1.0f;
                range = 1.5f;
                break;
        }

        circleCollider.radius = range;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out Health entityHealth))
            {
                float currentTime = Time.time;
                if (!enemyLastHitTime.ContainsKey(collision.gameObject) || currentTime - enemyLastHitTime[collision.gameObject] >= damageRate)
                {
                    entityHealth.TakeDamage((int)damage);
                    enemyLastHitTime[collision.gameObject] = currentTime; 
                    Debug.Log($"{collision.gameObject.name} took {damage} damage from garlic");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyLastHitTime.ContainsKey(collision.gameObject))
        {
            enemyLastHitTime.Remove(collision.gameObject);
        }
    }
    private void GetGarlicLevel()
    {
        lvl = xpSystem.garlicLvl;
    }
}