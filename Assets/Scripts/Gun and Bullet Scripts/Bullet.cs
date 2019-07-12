using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Not serializing bullet values. Should be derived from GunProperties")]
    public Rigidbody2D rigidBody2D;
    [System.NonSerialized]
    public float bulletSpeed;
    [System.NonSerialized]
    public float bulletDamage;
    [System.NonSerialized]
    public float timeBulletSelfDestruct;
    [System.NonSerialized]
    public float knockBack;
    [System.NonSerialized]
    public float bulletAccuracy;
    [System.NonSerialized]
    public float bulletAngle;
    [System.NonSerialized]
    public bool bulletBounce;
    [System.NonSerialized]
    public bool isExplosive;
    [System.NonSerialized]
    public float explosionDamage;
    [System.NonSerialized]
    public float explosiveForce;
    [System.NonSerialized]
    public float explosiveRadius;
    [System.NonSerialized]
    public Vector2 bulletTrajectory;
    [System.NonSerialized]
    public GameObject explosionEffect;

    [Header("Serialized bullet values. NOT Derived from Gun Properties")]
    public Sprite bulletSprite;
    private int bounces = 0;
    public int bulletBounceMaxNum;

    protected void OnEnable()
    {
        bounces = 0;
        bulletSprite = GetComponent<SpriteRenderer>().sprite;
        rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(SetInactiveSelf());
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
                gameObject.SetActive(false);
            }
        }
        if (!bulletBounce)
        {
            gameObject.SetActive(false);
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
    protected Vector2 bulletDirection()
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
    //Note: Do not need to reset, because now that Player and Enemy set bullet properties, they will reset when shot again
    public void setBulletProperties(float bulletSpeed, float bulletDamage, float timeBulletSelfDestruct, float knockBack, float bulletAccuracy, float bulletAngle, bool bulletBounce, int bulletBounceMaxNum, bool isExplosive, float explosionDamage, float explosiveForce, float explosiveRadius, GameObject explosionEffect)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.timeBulletSelfDestruct = timeBulletSelfDestruct;
        this.knockBack = knockBack;
        this.bulletAccuracy = bulletAccuracy;
        this.bulletAngle = bulletAngle;
        this.bulletBounce = bulletBounce;
        this.bulletBounceMaxNum = bulletBounceMaxNum;
        this.isExplosive = isExplosive;
        this.explosionDamage = explosionDamage;
        this.explosiveForce = explosiveForce;
        this.explosiveRadius = explosiveRadius;
        this.explosionEffect = explosionEffect;
    }
    protected IEnumerator SetInactiveSelf()
    {
        yield return new WaitForSeconds(timeBulletSelfDestruct);
        gameObject.SetActive(false);
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
