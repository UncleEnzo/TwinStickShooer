﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    BoxCollider2D boxCollider;
    float reflectBulletSpeed = 14f;
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
            }
        }

        //Parry Mechanic Trigger
        if (readyToSwipe && Input.GetKeyDown(KeyCode.E) && !InventoryUI.UIOpen)
        {
            parry();
        }
        else if (!readyToSwipe && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Parry on cooldown");
        }
    }

    void parry()
    {
        //paints a single target for all parry bullets to attack
        Transform enemyTransform = null;

        //Perform the parry
        foreach (Collider2D collider in bulletsInCollider)
        {
            if (collider.gameObject.activeInHierarchy)
            {
                if (enemyTransform == null)
                {
                    enemyTransform = UbhUtil.GetTransformFromTagName(TagsAndLabels.EnemyTag, false, true, collider.gameObject.transform);
                }
                UbhBullet enemyBullet = collider.gameObject.GetComponent<UbhBullet>();
                enemyBullet.m_pauseAndResume = false;
                enemyBullet.m_accelSpeed = 0;
                collider.gameObject.tag = TagsAndLabels.PlayerBulletTag;
                collider.gameObject.layer = LayerMask.NameToLayer(TagsAndLabels.PlayerBulletLabel);
                enemyBullet.rbMovement = true;
                enemyBullet.isRbTrajConfigured = false;
                //note: using mathf.abs to ensure that negative speeds don't cause issues (neg speeds set for decelerating bullets)
                Vector2 currentTrajectory = (enemyTransform.position - collider.gameObject.transform.position) / Mathf.Abs(enemyBullet.m_speed);
                enemyBullet.m_speed = reflectBulletSpeed;
                enemyBullet.m_bulletTrajectory = currentTrajectory * enemyBullet.m_speed;
            }
        }

        //removes parried bullets from list because otherwise they never exit collider
        bulletsInCollider.Clear();

        readyToSwipe = false;
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
