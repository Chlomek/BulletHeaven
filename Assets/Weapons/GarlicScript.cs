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

        switch (lvl)
        {
            case 0:
                gameObject.SetActive(false);
                Debug.Log("Garlic weapon deactivated (level 0)");
                break;
            case 1:
                damage = 2;
                damageRate = 1.0f;
                range = 1.5f;
                Debug.Log($"Garlic weapon set to level 1: {damage} damage, 1,5 radius");
                break;

            case 2:
                damage = 2;
                damageRate = 1.0f;
                range = 1.8f;
                Debug.Log($"Garlic weapon set to level 2: {damage} damage, 1.8 radius");
                break;
            case 3:
                damage = 3;
                damageRate = 1.0f;
                range = 1.8f;
                Debug.Log($"Garlic weapon set to level 3: {damage} damage, 1.8 radius");
                break;
            case 4:
                damage = 3;
                damageRate = 0.8f;
                range = 1.8f;
                Debug.Log($"Garlic weapon set to level 4: {damage} damage, 1.8 radius, faster tick rate");
                break;
            case 5:
                damage = 3;
                damageRate = 0.8f;
                range = 2.25f;
                Debug.Log($"Garlic weapon set to level 5: {damage} damage, 2.25 radius, faster tick rate");
                break;
            default:
                damage = 2;
                damageRate = 1.0f;
                range = 1.5f;
                Debug.Log($"Garlic weapon defaulted to level 1 settings: {damage} damage, 1.5 radius");
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