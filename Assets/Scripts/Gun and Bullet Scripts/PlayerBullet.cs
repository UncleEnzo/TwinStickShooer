﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    new void OnEnable()
    {
        base.OnEnable();
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
}
