using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage = 1f;
    public float timeBulletSelfDestruct = 3f;
    public float knockBack = 300f;
    Rigidbody2D rigidBody2D;

    //bullet accuracy (Use for spread weapons as well) 0F equals perfect accuracy
    public float bulletAccuracy = 0f;

    //bullet angle
    public float bulletAngle = 0f;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        Vector2 movementDirection = bulletDirection();
        if (GetComponentInParent<Player>())
        {
            rigidBody2D.velocity = movementDirection * speed;
        }
        else
        {
            rigidBody2D.velocity = transform.right * speed;
        }
        destroySelf();
    }

    public Vector2 bulletDirection()
    {
        // Take the random angle variation and add it to the initial
        // desiredDirection (which we convert into another angle), which in this case is the players aiming direction
        var x = transform.position.x - GameObject.Find("Player").transform.position.x;
        var y = transform.position.y - GameObject.Find("Player").transform.position.y;
        float rotateAngle = bulletAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);

        // Calculate the new direction we will move in which takes into account 
        // the random angle generated
        return new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
    }

    public Vector2 accuracy()
    {
        // Randomize angle variation between bullets
        float spreadAngle = Random.Range(-bulletAccuracy, bulletAccuracy);

        // Take the random angle variation and add it to the initial
        // desiredDirection (which we convert into another angle), which in this case is the players aiming direction
        var x = transform.position.x - GameObject.Find("Player").transform.position.x;
        var y = transform.position.y - GameObject.Find("Player").transform.position.y;
        float rotateAngle = spreadAngle + (Mathf.Atan2(y, x) * Mathf.Rad2Deg);

        // Calculate the new direction we will move in which takes into account 
        // the random angle generated
        return new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
    }

    private void destroySelf()
    {
        Destroy(gameObject, timeBulletSelfDestruct);
    }
}
