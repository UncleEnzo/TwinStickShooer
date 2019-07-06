using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Pistol, Shotgun, AssaultRifle, SniperRifle, MachineGun, RocketLaucher, RailGun, Knife, StarShatter }

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
    [SerializeField]
    public float bulletSpeed = 15f;
    [SerializeField]
    public float bulletDamage = 1f;
    [SerializeField]
    public float timeBulletSelfDestruct = 3f;
    [SerializeField]
    public float knockBack = 5;
    [SerializeField]
    public float bulletAccuracy = 0f; //0 = perfect accuracy
    [SerializeField]
    public float bulletAngle = 0f;
    [SerializeField]
    public bool bulletBounce = false;
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
        lookAtPoint(target, Wielder);
        rotateAroundShoulder(target, armLength, Wielder);
    }

    private void lookAtPoint(Vector3 target, Vector3 Wielder)
    {
        Vector3 dir = target - Wielder;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rotateAroundShoulder(Vector3 target, float armLength, Vector3 Wielder)
    {
        Vector3 shoulderToMouseDir = target - Wielder;
        shoulderToMouseDir.z = 0;
        transform.position = Wielder + (armLength * shoulderToMouseDir.normalized);
    }
}
