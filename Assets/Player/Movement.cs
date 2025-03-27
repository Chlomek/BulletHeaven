using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1;
    public Vector2 lookDirection = Vector2.right;

    public float worldWidth = 500f;
    public float worldHeight = 500f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

        Vector2 playerPosition = transform.position;
        float clampedX = Mathf.Clamp(playerPosition.x, -worldWidth / 2f, worldWidth / 2f);
        float clampedY = Mathf.Clamp(playerPosition.y, -worldHeight / 2f, worldHeight / 2f);

        transform.position = new Vector2(clampedX, clampedY);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            lookDirection = new Vector2(horizontalInput, verticalInput).normalized;
        }
    }
}

