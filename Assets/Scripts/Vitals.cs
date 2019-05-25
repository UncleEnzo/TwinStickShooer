using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitals : MonoBehaviour
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
        if (collidingObject.gameObject.tag == "EnemyBullet" && gameObject.tag == "Player")
        {
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
        if (gameObject.tag == ("Player") && collision.gameObject.tag == "EnemyBullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
        if (gameObject.tag == ("Player") && collision.gameObject.tag == "Enemy")
        {
            health -= collision.gameObject.GetComponent<Enemy>().walkDamageToPlayer;
        }
        if (gameObject.tag == ("Enemy") && collision.gameObject.tag == "PlayerBullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}