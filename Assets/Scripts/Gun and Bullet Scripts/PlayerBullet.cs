using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{

    new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(SetInactiveSelf());
        bulletTrajectory = bulletDirection();
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
    }

    new void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.EnemyTag)
        {
            if (!isExplosive)
            {
                collisionInfo.gameObject.GetComponent<Enemy>().enemyTrajectory = Vector2.zero;
                collisionInfo.gameObject.GetComponent<Enemy>().hit(bulletDamage, knockBack, bulletTrajectory);
            }
        }
        base.OnCollisionEnter2D(collisionInfo);
    }

    protected IEnumerator SetInactiveSelf()
    {
        yield return new WaitForSeconds(timeBulletSelfDestruct);
        gameObject.SetActive(false);
    }
    private Vector2 bulletDirection()
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
}
