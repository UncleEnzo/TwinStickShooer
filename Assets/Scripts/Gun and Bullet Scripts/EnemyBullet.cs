using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    new void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            collisionInfo.gameObject.GetComponent<Player>().hit(bulletDamage);
        }
        base.OnCollisionEnter2D(collisionInfo);
    }
}
