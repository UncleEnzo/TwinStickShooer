using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Not serializing bullet values. Should be derived from GunProperties")]
    public Rigidbody2D rigidBody2D;
    [System.NonSerialized]
    public float bulletSpeed;
    [System.NonSerialized]
    public float bulletDamage;
    [System.NonSerialized]
    public float timeBulletSelfDestruct;
    [System.NonSerialized]
    public float knockBack;
    [System.NonSerialized]
    public float bulletAccuracy;
    [System.NonSerialized]
    public float bulletAngle;
    [System.NonSerialized]
    public bool bulletBounce;
    [System.NonSerialized]
    public Vector2 bulletTrajectory;
    protected void OnRenderObject()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(SetInactiveSelf());
    }
    protected void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.ChestLabel) && bulletBounce == false)
        {
            collisionInfo.gameObject.GetComponent<TreasureChest>().health--;
        }
        gameObject.SetActive(false);
    }
    protected Vector2 bulletDirection()
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
    //Note: Do not need to reset, because now that Player and Enemy set bullet properties, they will reset when shot again
    public void setBulletProperties(float bulletSpeed, float bulletDamage, float timeBulletSelfDestruct, float knockBack, float bulletAccuracy, float bulletAngle, bool bulletBounce)
    {
        this.bulletSpeed = bulletSpeed;
        this.bulletDamage = bulletDamage;
        this.timeBulletSelfDestruct = timeBulletSelfDestruct;
        this.knockBack = knockBack;
        this.bulletAccuracy = bulletAccuracy;
        this.bulletAngle = bulletAngle;
        this.bulletBounce = bulletBounce;
    }

    protected IEnumerator SetInactiveSelf()
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
