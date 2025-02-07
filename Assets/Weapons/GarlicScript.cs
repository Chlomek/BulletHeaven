using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicScript : MonoBehaviour
{
    [SerializeField]
    private int damage = 2;
    [SerializeField]
    private float damageRate = 1f;  // Damage every second
    private Dictionary<GameObject, float> enemyLastHitTime = new Dictionary<GameObject, float>();

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<Health>(out Health entityHealth))
            {
                float currentTime = Time.time;

                // Check if the enemy was last hit within the cooldown time
                if (!enemyLastHitTime.ContainsKey(collision.gameObject) || currentTime - enemyLastHitTime[collision.gameObject] >= damageRate)
                {
                    entityHealth.TakeDamage(damage);
                    enemyLastHitTime[collision.gameObject] = currentTime;  // Update last hit time
                    Debug.Log(collision.gameObject.name + " took " + damage + " damage from garlic");
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
}
