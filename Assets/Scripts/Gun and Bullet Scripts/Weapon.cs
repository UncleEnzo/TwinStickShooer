using System;
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
    [Range(.1f, 10f)] public float camShakeMagnitude = .1f;
    [SerializeField]
    [Range(.01f, .1f)] public float camShakeLength = .03f;
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
    public bool destroyBulletsOnDeath = true;
    [SerializeField]
    public float bulletDamage = 1f;
    [SerializeField]
    public float bulletSpeed = 10f;
    [SerializeField]
    public float timeBulletSelfDestruct = 10f;
    [SerializeField]
    public float knockBack = 5f;
    [SerializeField]
    public float bulletAccuracy = 0f; //0f = perfect accuracy
    [SerializeField]
    public bool isExplosiveRecipe = false;
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
    protected Vector3 mousePosTarget;
    protected Transform playerTransform;
    [NonSerializedAttribute]
    public UbhShowcaseCtrl shotControllerShowCase;

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

    protected void Update()
    {
        mousePosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerTransform = Player.Instance.transform;
        float angle = lookAtPoint(mousePosTarget, playerTransform.position);
        //Player weapon auto updates angle for any bulletPattern script
        if (gameObject.transform.IsChildOf(WeaponSwitching.Instance.gameObject.transform))
        {
            foreach (var shotInfo in shotControllerShowCase.activeShotCtrl.m_shotList)
            {
                shotInfo.m_shotObj.GetComponent<UbhBaseShot>().m_angle = angle;
            }
        }
    }

    protected void ApplyGunProperties(UbhBaseShot baseShot)
    {
        baseShot.m_bulletTag = GunProperties.bulletTag;
        baseShot.m_destroyBulletsOnDeath = GunProperties.destroyBulletsOnDeath;
        baseShot.m_damage = GunProperties.bulletDamage;
        baseShot.m_bulletSpeed = GunProperties.bulletSpeed;
        baseShot.m_knockBack = GunProperties.knockBack;
        baseShot.m_bulletAccuracy = GunProperties.bulletAccuracy; //Not sure about this one
        baseShot.m_isExplosiveRecipe = GunProperties.isExplosiveRecipe;
        baseShot.m_autoReleaseTime = GunProperties.timeBulletSelfDestruct;
        baseShot.m_isBulletBounce = GunProperties.isBulletBounce;
        baseShot.m_bulletBounceMaxNum = GunProperties.bulletBounceMaxNum;
        baseShot.m_isExplosive = GunProperties.isExplosive;
        baseShot.m_explosionDamage = GunProperties.explosionDamage;
        baseShot.m_explosiveForce = GunProperties.explosiveForce;
        baseShot.m_explosiveRadius = GunProperties.explosiveRadius;
        baseShot.m_explosionEffect = GunProperties.explosionEffect;
        baseShot.m_explosionEffect = GunProperties.explosionEffect;
    }
}
