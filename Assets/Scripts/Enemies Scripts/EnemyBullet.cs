using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 15f;
    public float bulletDamage = 1f;
    public float timeBulletSelfDestruct = 3f;
    public float knockBack = 300f;
    Rigidbody2D rigidBody2D;
    public float bulletAngle = 0f;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.velocity = transform.right * bulletSpeed;
        destroySelf();
    }
    private void destroySelf()
    {
        Destroy(gameObject, timeBulletSelfDestruct);
    }
}
