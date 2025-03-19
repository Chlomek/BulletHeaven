using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicScript : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float damageRate = 1f;  // Damage every second

    [Header("Level Settings")]
    [SerializeField] private int maxLevel = 5;

    private Dictionary<GameObject, float> enemyLastHitTime = new Dictionary<GameObject, float>();
    private XPSystem xpSystem;  // Reference to the XP system
    private CircleCollider2D circleCollider;  // Reference to the circle collider
    private int lvl = 0;
    private float currentDamage = 0f;

    void Start()
    {
        // Get reference to the XP system
        xpSystem = FindObjectOfType<XPSystem>();
        if (xpSystem == null)
        {
            Debug.LogError("XPSystem not found in the scene!");
        }

        // Get reference to circle collider
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogError("CircleCollider2D not found on the garlic weapon!");
        }

        // Initial check to enable/disable based on level
        UpdateGarlicLevel();
    }

    void Update()
    {
        // Check if garlic level has changed
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
                return;
            case 1:
                currentDamage = 2;
                damageRate = 1.0f;
                if (circleCollider != null) circleCollider.radius = 1.5f;
                Debug.Log($"Garlic weapon set to level 1: {currentDamage} damage, 1,5 radius");
                break;

            case 2:
                currentDamage = 2;
                damageRate = 1.0f; // Same tick rate
                if (circleCollider != null) circleCollider.radius = 1.8f; // +20% area of effect
                Debug.Log($"Garlic weapon set to level 2: {currentDamage} damage, 1.8 radius");
                break;
            case 3:
                currentDamage = 3;
                damageRate = 1.0f; // Same tick rate
                if (circleCollider != null) circleCollider.radius = 1.8f; // Same radius
                Debug.Log($"Garlic weapon set to level 3: {currentDamage} damage, 1.8 radius");
                break;
            case 4:
                currentDamage = 3; // Same damage
                damageRate = 0.8f; // Increased tick rate (lower value = faster ticks)
                if (circleCollider != null) circleCollider.radius = 1.8f; // Same radius
                Debug.Log($"Garlic weapon set to level 4: {currentDamage} damage, 1.8 radius, faster tick rate");
                break;
            case 5:
                currentDamage = 3; // Same damage
                damageRate = 0.8f; // Same tick rate
                if (circleCollider != null) circleCollider.radius = 2.25f; // +25% area of effect
                Debug.Log($"Garlic weapon set to level 5: {currentDamage} damage, 2.25 radius, faster tick rate");
                break;
            default:
                // Fallback for unexpected levels
                currentDamage = 2;
                damageRate = 1.0f;
                if (circleCollider != null) circleCollider.radius = 1.5f;
                Debug.Log($"Garlic weapon defaulted to level 1 settings: {currentDamage} damage, 1.5 radius");
                break;
        }

        // You could also update visual effects based on level here
        // UpdateVisualEffects();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out Health entityHealth))
            {
                float currentTime = Time.time;
                // Check if the enemy was last hit within the cooldown time
                if (!enemyLastHitTime.ContainsKey(collision.gameObject) || currentTime - enemyLastHitTime[collision.gameObject] >= damageRate)
                {
                    entityHealth.TakeDamage((int)currentDamage);
                    enemyLastHitTime[collision.gameObject] = currentTime;  // Update last hit time
                    Debug.Log($"{collision.gameObject.name} took {currentDamage} damage from garlic");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyLastHitTime.ContainsKey(collision.gameObject))
        {
            enemyLastHitTime.Remove(collision.gameObject);  // Remove from dictionary when enemy leaves range
        }
    }
    private void GetGarlicLevel()
    {
        lvl = GameObject.Find("Player").GetComponent<XPSystem>().garlicLvl;
    }
}