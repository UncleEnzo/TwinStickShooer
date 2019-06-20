using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    private Enemy enemy;
    public GameObject greenCraftComponent;
    public GameObject purpleCraftComponent;
    public GameObject blackCraftComponent;
    public int minDropCount = 0;
    public int maxDropCount = 7;
    public float minDropDist = 2f;
    public float maxDropDist = 2f;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public void OnCollisionEnter2D(Collision2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "PlayerBullet" && gameObject.tag == "Enemy")
        {
            collidingObject.gameObject.SetActive(false);
        }
        takeDamage(collidingObject);
        die();
    }
    private void die()
    {
        if (enemy.health <= 0f)
        {
            //Play some animation, particles, and sounds
            dropCraftComponents();
            FindObjectOfType<EnemyRoom>().reduceEnemyCount();
            gameObject.SetActive(false);
        }
    }
    private void takeDamage(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            enemy.health -= collision.gameObject.GetComponent<PlayerBullet>().getBulletDamage();
        }
    }

    private void dropCraftComponents()
    {
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(greenCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(purpleCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(blackCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
    }

    private float randomDistFromEnemy(float pos)
    {
        return Random.Range(pos - minDropDist, pos + maxDropDist);
    }
}