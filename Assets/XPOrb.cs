using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float attractionDistance = 5f;
    public float attractionSpeed = 2f;
    public int xpAmount = 1;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attractionDistance)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Assuming you have a Player script with an AddXP method
            collision.GetComponent<XPSystem>().AddXP(xpAmount); // Add XP to the player
            Destroy(gameObject); // Destroy the XP orb
        }
    }
}
