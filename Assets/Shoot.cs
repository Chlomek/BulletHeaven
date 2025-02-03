using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Shoot : MonoBehaviour
{
    public float firerate = 1;
    public GameObject bullet;
    private GameObject myTarget;
    private float lastShot = 0.0f;
    private GameObject[] targets;
    public float bulletSpeed = 10;

    private int lvl;

    void start()
    {
        lvl = GameObject.Find("Player").GetComponent<XPSystem>().level;
        //lvl = GetComponent<XPSystem>().level;
    }

    public void ShootBullet()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
        Vector2 direction = (myTarget.transform.position - transform.position).normalized;
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        newBullet.GetComponent<ProjectileBehavior>().SetDirection(direction); // Set direction for rotation
    }

    // Update is called once per frame
    void Update()
    {
        lvl = GameObject.Find("Player").GetComponent<XPSystem>().level;
        firerate = 1f / lvl;
        /*
        switch(lvl)
        {
            case 1:
                firerate = 1;
                break;
            case 2:
                firerate = 0.5f;
        }
        */
        if (Time.timeSinceLevelLoad - lastShot >= firerate)
        {
            try
            {
                targets = GameObject.FindGameObjectsWithTag("Enemy");
                myTarget = targets.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
            }
            catch
            {
                return;
            }
            ShootBullet();
            lastShot = Time.timeSinceLevelLoad;
        }
    }
}

