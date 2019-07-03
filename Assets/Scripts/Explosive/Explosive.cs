using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    private bool hasExploded = false;
    public GameObject explosionEffect;
    public float explosiveRadius;
    private float explosiveForce;
    private float explosionDamage;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    public void SetExplosiveProperties(float explosionDamage, float explosiveRadius, float explosiveForce)
    {
        this.explosionDamage = explosionDamage;
        this.explosiveRadius = explosiveRadius;
        this.explosiveForce = explosiveForce;
    }
    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.EnemyTag)
        {
            Explode();
        }
    }
    void Explode()
    {
        //create explosion
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
        CameraController.Instance.Shake((Player.Instance.transform.position - transform.position).normalized, 1f, .03f);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Destroys enemy bullets caught in the explosion
            if (!nearbyObject.isTrigger && nearbyObject.GetComponent<EnemyBullet>())
            {
                nearbyObject.gameObject.SetActive(false);
            }

            //Applies knockback
            if (!nearbyObject.isTrigger && nearbyObject.GetComponent<Rigidbody2D>())
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
        Destroy(gameObject);
    }
}
