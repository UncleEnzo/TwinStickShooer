using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitals : MonoBehaviour
{
    public float health;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        takeDamage(collision);
        die();
    }
    private void die()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void takeDamage(Collision2D collision)
    {
        if (gameObject.tag == ("Player") && collision.gameObject.tag == "EnemyBullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
        if (gameObject.tag == ("Enemy") && collision.gameObject.tag == "PlayerBullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}
