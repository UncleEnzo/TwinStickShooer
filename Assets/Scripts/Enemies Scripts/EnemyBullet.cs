using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float bulletDamage = 1f;
    public float timeBulletSelfDestruct = 3f;
    public float knockBack = 300f;
    Rigidbody2D rigidBody2D;
    public float bulletAngle = 0f;

    public bool bulletBounce = false;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.velocity = transform.right * bulletSpeed;
        destroySelf();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall" && bulletBounce == false)
        {
            Destroy(gameObject);
        }
    }
    private void destroySelf()
    {
        Destroy(gameObject, timeBulletSelfDestruct);
    }
}
