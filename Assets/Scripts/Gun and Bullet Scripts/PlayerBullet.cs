using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    private float bulletSpeed;
    private float bulletDamage;
    private float timeBulletSelfDestruct;
    private float knockBack;
    private float bulletAccuracy;
    private float bulletAngle;
    private Vector2 bulletTrajectory;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        bulletTrajectory = bulletDirection();
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
        destroySelf();
    }

    public Vector2 bulletDirection()
    {
        // Randomize angle variation between bullets
        float spreadAngle = Random.Range(-bulletAccuracy, bulletAccuracy);

        // Take the random angle variation and add it to the initial
        // desiredDirection (which we convert into another angle), which in this case is the players aiming direction
        var x = transform.right.x;
        var y = transform.right.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);

        // Calculate the new direction we will move in which takes into account 
        // the random angle generated
        return new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
    }

    public void setPlayerBulletProperties(float bulletSpeed, float bulletDamage, float timeBulletSelfDestruct, float knockBack, float bulletAccuracy, float bulletAngle)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.timeBulletSelfDestruct = timeBulletSelfDestruct;
        this.knockBack = knockBack;
        this.bulletAccuracy = bulletAccuracy;
        this.bulletAngle = bulletAngle;
    }

    private void destroySelf()
    {
        Destroy(gameObject, timeBulletSelfDestruct);
    }
    public float getBulletKnockBack()
    {
        return knockBack;
    }
    public float getBulletDamage()
    {
        return bulletDamage;
    }
    public Vector2 getBulletTrajectory()
    {
        return bulletTrajectory;
    }
}
