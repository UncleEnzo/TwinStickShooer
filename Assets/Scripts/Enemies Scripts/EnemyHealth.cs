using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject greenCraftComponent;
    public GameObject purpleCraftComponent;
    public GameObject blackCraftComponent;
    public int minDropCount = 0;
    public int maxDropCount = 7;
    public float minDropDist = 2f;
    public float maxDropDist = 2f;
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
            //Play some animation, particles, and sounds
            dropCraftComponents();
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

    private void dropCraftComponents()
    {
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            Instantiate<GameObject>(greenCraftComponent, new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y)), this.transform.rotation);
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            Instantiate<GameObject>(purpleCraftComponent, new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y)), this.transform.rotation);
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            Instantiate<GameObject>(blackCraftComponent, new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y)), this.transform.rotation);
        }
    }

    private float randomDistFromEnemy(float pos)
    {
        return Random.Range(pos - minDropDist, pos + maxDropDist);
    }
}