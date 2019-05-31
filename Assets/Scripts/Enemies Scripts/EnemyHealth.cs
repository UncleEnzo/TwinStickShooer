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
            health -= collision.gameObject.GetComponent<PlayerBullet>().getBulletDamage();
        }
    }
}