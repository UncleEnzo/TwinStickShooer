using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    BoxCollider2D boxCollider;
    private GameObject weaponHolder;
    private float coolDownOnParry = 3f;
    private float defaultCoolDownOnParry = 3f;
    public float coolDownResetValue;
    private bool readyToSwipe = true;
    private Sprite colliderSizeFromStack;
    private Animation parrySwipe;
    public float colliderSizeX;
    public float defaultColliderOffSetX = .5f;
    private float defaultcolliderSizeX = 1f;
    private float defaultcolliderSizeY = 2.5f;
    public GameObject bullet;
    private List<Collider2D> bulletsInCollider = new List<Collider2D>();

    void OnEnable()
    {
        //Returns object that you can cast to BoxCollider
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.enabled = true;

        //Set collider size > sizeOfCollider.
        boxCollider.size = new Vector2(defaultcolliderSizeX, defaultcolliderSizeY);
        boxCollider.offset = new Vector2(defaultColliderOffSetX, 0f);
    }

    void OnDisable()
    {
        print("CALLED");
        boxCollider.enabled = false;
        colliderSizeX = defaultcolliderSizeX;
        coolDownResetValue = defaultCoolDownOnParry;
        coolDownOnParry = defaultCoolDownOnParry;
    }

    public void updateBoxCollider()
    {
        boxCollider.GetComponent<BoxCollider2D>().size = new Vector2(colliderSizeX, defaultcolliderSizeY);
        boxCollider.offset = new Vector2((colliderSizeX / 3), 0f);
    }

    void Update()
    {
        //CoolDown for parry
        if (!readyToSwipe)
        {
            coolDownOnParry -= Time.deltaTime;
            if (coolDownOnParry <= 0)
            {
                readyToSwipe = true;
                coolDownOnParry = coolDownResetValue;
                print("COOLDOWN RESET: " + coolDownOnParry);
            }
        }

        //Parry Mechanic Trigger
        if (readyToSwipe && Input.GetKeyDown(KeyCode.E))
        {
            print("Parrying");
            //Perform the parry
            foreach (Collider2D collider in bulletsInCollider)
            {
                collider.gameObject.tag = TagsAndLabels.PlayerBulletTag;
                EnemyBullet enemyBullet = collider.gameObject.GetComponent<EnemyBullet>();
                Vector3 enemyBulletPos = enemyBullet.transform.position;
                Quaternion enemyBulletRotation = enemyBullet.transform.rotation;
                float reflectBulletSpeed = 1f;
                Vector2 trajectory = new Vector2(-enemyBullet.rigidBody2D.velocity.x, -enemyBullet.rigidBody2D.velocity.y);
                enemyBullet.gameObject.SetActive(false);
                GameObject playerBullet = ObjectPooler.SharedInstance.GetPooledObject(bullet.name + "(Clone)");
                if (playerBullet != null)
                {
                    playerBullet.transform.position = enemyBulletPos;
                    playerBullet.transform.rotation = enemyBulletRotation;
                    playerBullet.GetComponent<PlayerBullet>().setBulletProperties(enemyBullet.bulletSpeed, enemyBullet.bulletDamage, enemyBullet.timeBulletSelfDestruct, enemyBullet.knockBack, enemyBullet.bulletAccuracy, enemyBullet.bulletAngle, enemyBullet.bulletBounce, enemyBullet.isExplosive, enemyBullet.explosionDamage, enemyBullet.explosiveForce, enemyBullet.explosiveRadius, enemyBullet.explosionEffect);
                    playerBullet.SetActive(true);
                    playerBullet.GetComponent<SpriteRenderer>().sprite = playerBullet.GetComponent<SpriteRenderer>().sprite;
                    playerBullet.GetComponent<PlayerBullet>().bulletTrajectory = trajectory * reflectBulletSpeed;
                    playerBullet.GetComponent<Rigidbody2D>().velocity = playerBullet.GetComponent<PlayerBullet>().bulletTrajectory;
                }
            }
            readyToSwipe = false;
        }
        else if (!readyToSwipe && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Parry on cooldown");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if the object is not already in the list
        if (collider.tag == TagsAndLabels.EnemyBulletTag)
        {
            //add the object to the list
            bulletsInCollider.Add(collider);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit2D(Collider2D collider)
    {
        //if the object is in the list
        if (bulletsInCollider.Contains(collider))
        {
            //remove it from the list
            bulletsInCollider.Remove(collider);
        }
    }
}
