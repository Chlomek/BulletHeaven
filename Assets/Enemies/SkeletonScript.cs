using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField]
    private float moveSpeed = 4;
    [SerializeField]
    private int damage = 10;


    private Transform target;
    private Rigidbody skeleton;

    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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
