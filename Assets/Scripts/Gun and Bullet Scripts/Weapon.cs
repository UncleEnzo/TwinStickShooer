﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Pistol, Shotgun, AssaultRifle, SniperRifle, MachineGun, StarShatter }

[Serializable]
public class GunProperties
{
    [Header("Gun Properties")]
    [SerializeField]
    public WeaponType weaponType; //Defines the type of weapon this is
    [SerializeField]
    public Transform[] bulletSpawnPoint; //Array so can fire multiple bullets at once
    [SerializeField]
    [Range(.1f, 10f)] public float camShakeMagnitude = 1.5f;
    [SerializeField]
    [Range(.01f, .1f)] public float camShakeLength = .05f;
    [SerializeField]
    public float bulletsPerSecond = 5;
    [SerializeField]
    public int maxAmmo = 10;
    [SerializeField]
    public float reloadTime = 2f;

    [Header("Bullet Properties")]
    [Header("PlayerBullet tag = true | EnemyBullet tag = false")]
    [SerializeField]
    public bool bulletTag = true;
    [SerializeField]
    public float bulletDamage = 1f;
    [SerializeField]
    public float bulletSpeed = 10f;
    [SerializeField]
    public float timeBulletSelfDestruct = 10f;
    [SerializeField]
    public float knockBack = 5f;
    [SerializeField]
    public float bulletAccuracy = 0f; //0 = perfect accuracy
    [SerializeField]
    public bool isBulletBounce = false;
    public int bulletBounceMaxNum = 0;
    [SerializeField]
    public bool isExplosive = false;
    [SerializeField]
    public float explosionDamage = 3f;
    [SerializeField]
    public float explosiveForce = 4f;
    [SerializeField]
    public float explosiveRadius = 3f;
    [SerializeField]
    public GameObject explosionEffect;
}
public class Weapon : MonoBehaviour
{
    [SerializeField]
    public GunProperties GunProperties;
    [Header("Gun Variables")]
    public float armLength = .5f;
    protected bool gunFacingRight = true;

    protected void SpriteFlip(Transform Weilder, Vector3 target)
    {
        //Flips gunsprite over the Y axis
        if (Weilder.InverseTransformPoint(target).x > 0 && !gunFacingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = !gameObject.GetComponent<SpriteRenderer>().flipY;
            gunFacingRight = !gunFacingRight;
        }
        else if (Weilder.InverseTransformPoint(target).x < 0 && gunFacingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = !gameObject.GetComponent<SpriteRenderer>().flipY;
            gunFacingRight = !gunFacingRight;
        }
    }

    protected void Aim(Vector3 target, Vector3 Wielder)
    {
        float angle = lookAtPoint(target, Wielder);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotateAroundShoulder(target, armLength, Wielder);
    }

    protected float lookAtPoint(Vector3 target, Vector3 Wielder)
    {
        Vector3 dir = target - Wielder;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void rotateAroundShoulder(Vector3 target, float armLength, Vector3 Wielder)
    {
        Vector3 shoulderToMouseDir = target - Wielder;
        shoulderToMouseDir.z = 0;
        transform.position = Wielder + (armLength * shoulderToMouseDir.normalized);
    }
}
