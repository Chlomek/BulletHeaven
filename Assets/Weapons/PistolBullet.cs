using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;

    public float speed = 10f; // Speed of the bullet
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField]
    private int bulletDespawn = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        Destroy(gameObject, bulletDespawn);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.TryGetComponent<Health>(out Health entityHealth))
            {
                entityHealth.TakeDamage(damage);
                Debug.Log(collision.gameObject.name + " took " + damage + " damage");
            }
            Destroy(gameObject);
        }
    }
}
