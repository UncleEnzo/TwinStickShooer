using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    BoxCollider2D boxCollider;
    private GameObject weaponHolder;
    private float coolDownOnParry;
    private float defaultCoolDownOnParry;
    public float coolDownResetValue;
    private bool readyToSwipe = true;
    private Sprite colliderSizeFromStack;
    private Animation parrySwipe;
    public float colliderSizeX;
    private float defaultcolliderSizeX = 1f;
    private float defaultcolliderSizeY = 2.5f;
    private List<Collider2D> bulletsInCollider = new List<Collider2D>();

    void OnEnable()
    {
        weaponHolder = FindObjectOfType<SetGunPosition>().gameObject;
        //Returns object that you can cast to BoxCollider
        boxCollider = weaponHolder.AddComponent<BoxCollider2D>();

        //Make it a trigger collider
        boxCollider.isTrigger = true;

        //Set collider size > sizeOfCollider.
        boxCollider.size = new Vector2(defaultcolliderSizeX, defaultcolliderSizeY);
        boxCollider.offset = new Vector2(.5f, 0f);
    }

    void OnDisable()
    {
        Destroy(boxCollider);
        colliderSizeX = defaultcolliderSizeX;
        coolDownResetValue = defaultCoolDownOnParry;
        coolDownOnParry = defaultCoolDownOnParry;
    }

    void Update()
    {
        //CoolDown for parry
        if (!readyToSwipe)
        {
            coolDownOnParry -= Time.deltaTime;
            print("Cooling down");
            if (coolDownOnParry <= 0)
            {
                readyToSwipe = true;
            }
        }

        //Parry Mechanic Trigger
        if (readyToSwipe && Input.GetKeyDown("e"))
        {
            print("Parrying");
            print(readyToSwipe);
            //Perform the parry
            foreach (Collider2D collider in bulletsInCollider)
            {
                collider.gameObject.tag = TagsAndLabels.PlayerBulletTag;
                EnemyBullet bullet = collider.gameObject.GetComponent<EnemyBullet>();
                bullet.bulletSpeed = bullet.bulletSpeed + 5f;
                bullet.rigidBody2D.velocity = (bullet.bulletTrajectory * -1) * bullet.bulletSpeed;
            }
            coolDownOnParry = coolDownResetValue;
            readyToSwipe = false;
        }
        else if (!readyToSwipe && Input.GetButtonDown("e"))
        {
            Debug.Log("Parry on cooldown");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if the object is not already in the list
        if (collider.tag == TagsAndLabels.EnemyBulletTag && !bulletsInCollider.Contains(collider))
        {
            print("BOX COLLIDER HIT");

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
