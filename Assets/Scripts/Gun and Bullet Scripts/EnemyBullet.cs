using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    new void OnRenderObject()
    {
        base.OnRenderObject();
        bulletTrajectory = transform.right;
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
    }
    new void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            collisionInfo.gameObject.GetComponent<Player>().hit(bulletDamage, knockBack, bulletTrajectory);
        }
        resetBulletTagEnemy();
        base.OnCollisionEnter2D(collisionInfo);
    }
    //This prevents parrying from screwing up which bullets are which in the pooler
    protected void resetBulletTagEnemy()
    {
        this.gameObject.tag = TagsAndLabels.EnemyBulletTag;
    }
}
