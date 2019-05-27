using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 3f;


    public void OnTriggerEnter2D(Collider2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "PlayerBullet" && gameObject.tag == "Enemy")
        {
            //   Vector2 force = transform.position - collidingObject.collider.transform.position;
            //  force.Normalize();
            //   collidingObject.collider.GetComponent<Rigidbody2D>().AddForce(-force * knockBack);
            Destroy(collidingObject.gameObject);
        }
        takeDamage(collidingObject);
        die();
    }
    private void die()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void takeDamage(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}