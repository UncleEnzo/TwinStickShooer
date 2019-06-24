using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float bulletDamage = 1f;
    public float timeBulletSelfDestruct = 3f;
    public float knockBack = 5f;
    public float knockTime = .25f;
    Rigidbody2D rigidBody2D;
    public float bulletAngle = 0f;
    public Vector2 bulletTrajectory;

    public bool bulletBounce = false;
    void OnRenderObject()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        bulletTrajectory = transform.right;
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
        StartCoroutine(SetInactiveSelf());
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if ((collisionInfo.gameObject.tag == "Wall" || collisionInfo.gameObject.layer == LayerMask.NameToLayer("Door")) && bulletBounce == false)
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator SetInactiveSelf()
    {
        yield return new WaitForSeconds(timeBulletSelfDestruct);
        gameObject.SetActive(false);
    }
}
