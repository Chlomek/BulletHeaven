using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float lifetime = 5f;  // Time before the bat despawns

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        // Determine a direction to move (bat will cross the player)
        moveDirection = (target.position - transform.position).normalized;

        // Invert the direction to make the bat move across and out of view
        //moveDirection = new Vector2(-moveDirection.x, -moveDirection.y);

        // Start the bat's movement
        StartCoroutine(DespawnAfterTime());
    }

    void Update()
    {
        // Move the bat in the set direction
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<Health>(out Health playerHealth))
            {
                playerHealth.TakeDamage(damage);
                GameObject newHit = Instantiate(hitEffect, collision.transform.position, Quaternion.identity) as GameObject;
                Destroy(newHit, 2);
                Debug.Log(collision.gameObject.name + " took " + damage + " damage");
            }
        }
    }

    private IEnumerator DespawnAfterTime()
    {
        // Wait for the set lifetime, then destroy the bat
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
