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
    private float lifetime = 5f;
    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        moveDirection = (target.position - transform.position).normalized;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, moveSpeed * Time.deltaTime);
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
}
