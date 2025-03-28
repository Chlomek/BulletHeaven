using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float attractionDistance = 5f;
    public float attractionSpeed = 2f;
    public int xpAmount = 1;
    public int lifeTime = 30;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attractionDistance)
        {
            float t = 1f - Mathf.Exp(-attractionSpeed * Time.deltaTime);

            transform.position = Vector2.Lerp(transform.position, player.position, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<XPSystem>().AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}
