using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    private float bulletSpeed;
    private float bulletDamage;
    private float timeBulletSelfDestruct;
    private float knockBack;
    private float bulletAccuracy;

    private float bulletAngle;
    private bool bulletBounce;
    private Vector2 bulletTrajectory;

    void OnRenderObject()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        bulletTrajectory = bulletDirection();
        rigidBody2D.velocity = bulletTrajectory * bulletSpeed;
        StartCoroutine(SetInactiveSelf());
    }


    public Vector2 bulletDirection()
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

    public void setPlayerBulletProperties(float bulletSpeed, float bulletDamage, float timeBulletSelfDestruct, float knockBack, float bulletAccuracy, float bulletAngle, bool bulletBounce)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.timeBulletSelfDestruct = timeBulletSelfDestruct;
        this.knockBack = knockBack;
        this.bulletAccuracy = bulletAccuracy;
        this.bulletAngle = bulletAngle;
        this.bulletBounce = bulletBounce;
    }

    private void resetBulletProperties()
    {
        float bulletSpeed;
        float bulletDamage;
        float timeBulletSelfDestruct;
        float knockBack;
        float bulletAccuracy;

        float bulletAngle;
        bool bulletBounce;
        Vector2 bulletTrajectory;
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.WallTag && bulletBounce == false)
        {
            gameObject.SetActive(false);
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.DoorLabel) && bulletBounce == false)
        {
            gameObject.SetActive(false);
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.ChestLabel) && bulletBounce == false)
        {
            collisionInfo.gameObject.GetComponent<TreasureChest>().health--;
            gameObject.SetActive(false);
        }
        else if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.ChestLabel) && bulletBounce == true)
        {
            //Do Damage
            //Bullet should bounce
            //Need to make a bullet bounce alternative for all collisions
        }
    }

    IEnumerator SetInactiveSelf()
    {
        yield return new WaitForSeconds(timeBulletSelfDestruct);
        gameObject.SetActive(false);
    }

    public float getBulletKnockBack()
    {
        return knockBack;
    }
    public float getBulletDamage()
    {
        return bulletDamage;
    }
    public Vector2 getBulletTrajectory()
    {
        return bulletTrajectory;
    }
}
