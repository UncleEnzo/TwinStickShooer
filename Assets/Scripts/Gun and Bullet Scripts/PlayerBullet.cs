using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    new void OnRenderObject()
    {
        base.OnRenderObject();
        bulletTrajectory = bulletDirection();
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
    }

    new void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.EnemyTag)
        {
            //damage ENEMY by calling hit.
            print("DAMAGING ENEMY");
        }
        resetBulletTagPlayer();
        base.OnCollisionEnter2D(collisionInfo);
    }

    //This prevents parrying from screwing up which bullets are which in the pooler
    private void resetBulletTagPlayer()
    {
        this.gameObject.tag = TagsAndLabels.PlayerBulletTag;
    }
}
