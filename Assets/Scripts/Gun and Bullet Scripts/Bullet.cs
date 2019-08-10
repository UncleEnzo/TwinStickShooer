using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Set these values manually if does not belong to a gun")]
    [Header("If it belongs to an enemyGun, configure these values and Uni Properities")]
    public Rigidbody2D rigidBody2D;
    public float bulletDamage;
    public float bulletSpeed;
    public float knockBack;
    public float bulletAccuracy;
    public float timeBulletSelfDestruct;
    public bool bulletBounce;
    public bool isExplosive;
    public float explosionDamage;
    public float explosiveForce;
    public float explosiveRadius;
    public Vector2 bulletTrajectory;
    public GameObject explosionEffect;

    [Header("Serialized bullet values. NOT Derived from Gun Properties")]
    public Sprite bulletSprite;
    private int bounces = 0;
    public int bulletBounceMaxNum;
    private UbhBullet bullet;

    protected void OnEnable()
    {
        bounces = 0;
        bulletSprite = GetComponent<SpriteRenderer>().sprite;
        rigidBody2D = GetComponent<Rigidbody2D>();
        bullet = GetComponent<UbhBullet>();
    }

    protected void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.ChestLabel))
        {
            collisionInfo.gameObject.GetComponent<TreasureChest>().health--;
        }
        if (isExplosive)
        {
            explosiveBullet();
        }
        if (bulletBounce)
        {
            //Recalculating bullet trajectory for next hit :)
            bulletTrajectory = (rigidBody2D.velocity / bulletSpeed);
            bounces++;
            if (bounces >= bulletBounceMaxNum)
            {
                if (bullet != null && bullet.isActive)
                {
                    UbhObjectPool.instance.ReleaseBullet(bullet);
                }
            }
        }
        if (!bulletBounce)
        {
            if (bullet != null && bullet.isActive)
            {
                UbhObjectPool.instance.ReleaseBullet(bullet);
            }
        }
    }
    protected void explosiveBullet()
    {
        //create explosion
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Note: only reinstate this if you will be only using player explosive bullets
            // //Destroys enemy bullets caught in the explosion
            // if (!nearbyObject.isTrigger && nearbyObject.GetComponent<EnemyBullet>())
            // {
            //     nearbyObject.gameObject.SetActive(false);
            // }

            //Applies explosion
            if (nearbyObject.tag != TagsAndLabels.PlayerBulletTag && !nearbyObject.isTrigger && nearbyObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                Vector2 difference = rb.transform.position - transform.position;
                difference = difference * explosiveForce;
                if (rb.GetComponent<Player>())
                {
                    rb.GetComponent<Player>().hit(0, explosiveForce, difference);
                }
                //Applies explosive damage to the enemy
                if (rb.GetComponent<Enemy>())
                {
                    rb.GetComponent<Enemy>().enemyTrajectory = Vector2.zero;
                    rb.GetComponent<Enemy>().hit(explosionDamage, explosiveForce, difference);
                }
                else
                {
                    rb.AddForce(difference, ForceMode2D.Impulse);
                }
            }
        }
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
    }

    //Note: Do not need to reset properties, because now that Player and Enemy set bullet properties, they will reset when shot again
    public void setBulletProperties(float bulletSpeed, float bulletDamage, float timeBulletSelfDestruct, float knockBack, float bulletAccuracy, float bulletAngle, bool bulletBounce, int bulletBounceMaxNum, bool isExplosive, float explosionDamage, float explosiveForce, float explosiveRadius, GameObject explosionEffect)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.timeBulletSelfDestruct = timeBulletSelfDestruct;
        this.knockBack = knockBack;
        this.bulletAccuracy = bulletAccuracy;
        this.bulletBounce = bulletBounce;
        this.bulletBounceMaxNum = bulletBounceMaxNum;
        this.isExplosive = isExplosive;
        this.explosionDamage = explosionDamage;
        this.explosiveForce = explosiveForce;
        this.explosiveRadius = explosiveRadius;
        this.explosionEffect = explosionEffect;
    }
}
