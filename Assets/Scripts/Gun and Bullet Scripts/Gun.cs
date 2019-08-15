using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : Weapon
{
    private int currentAmmo;
    protected bool isPlayer;
    protected float lastfired;
    protected bool isReloading = false;
    protected GameObject player;
    protected Vector3 mousePosTarget;
    protected Transform playerTransform;
    [Header("Sound effects")]
    protected AudioSource gunSounds;
    public AudioClip gunShotSound;
    public AudioClip gunReloadSound;

    [NonSerializedAttribute]
    public UbhShowcaseCtrl shotControllerShowCase;

    protected void Start()
    {
        gunSounds = GetComponent<AudioSource>();
        shotControllerShowCase = GetComponent<UbhShowcaseCtrl>();
        currentAmmo = GunProperties.maxAmmo;
        player = Player.Instance.transform.gameObject;
        PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    private void onDisable()
    {
        StopCoroutine(reload());
    }

    protected void FireGun(bool isPlayer)
    {
        if (isReloading)
        {
            return;
        }
        if (isPlayer)
        {
            if (Input.GetKeyDown("r") && currentAmmo < GunProperties.maxAmmo)
            {
                StartCoroutine(reload());
                return;
            }
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(reload());
            return;
        }
        if (isPlayer)
        {
            FirePlayerGun();
        }
        else if (!isPlayer)
        {
            FireEnemyGun();
        }
    }

    private void FirePlayerGun()
    {
        if (Input.GetMouseButton(0) && (Time.time - lastfired) > (1 / GunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            bool isShooting = Shoot();
            if (!isShooting)
            {
                gunSounds.PlayOneShot(gunShotSound);
                currentAmmo--;
                PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
                CameraController.Instance.Shake((player.transform.position - transform.position).normalized,
                                                GunProperties.camShakeMagnitude, GunProperties.camShakeLength);
            }
        }
    }

    private void FireEnemyGun()
    {
        if ((Time.time - lastfired) > (1 / GunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            //note: Enemy does not need aim since it uses lock on Patterns
            bool isShooting = Shoot();
            if (!isShooting)
            {
                gunSounds.PlayOneShot(gunShotSound);
                currentAmmo--;
            }
        }
    }

    private bool Shoot()
    {
        //Update bullets with gun Properties
        foreach (var shotInfo in shotControllerShowCase.activeShotCtrl.m_shotList)
        {
            ApplyGunProperties(shotInfo.m_shotObj.GetComponent<UbhBaseShot>());
        }
        bool isShooting = shotControllerShowCase.activeShotCtrl.shooting;
        if (!isShooting)
        {
            shotControllerShowCase.activeShotCtrl.StartShotRoutine();
        }
        return isShooting;
    }

    private void ApplyGunProperties(UbhBaseShot baseShot)
    {
        baseShot.m_bulletTag = GunProperties.bulletTag;
        baseShot.m_damage = GunProperties.bulletDamage;
        baseShot.m_bulletSpeed = GunProperties.bulletSpeed;
        baseShot.m_knockBack = GunProperties.knockBack;
        baseShot.m_bulletAccuracy = GunProperties.bulletAccuracy; //Not sure about this one
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

    protected void Update()
    {
        mousePosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerTransform = Player.Instance.transform;
        float angle = lookAtPoint(mousePosTarget, playerTransform.position);
        angle -= 90;

        //Player weapon auto updates angle for any bulletPattern script
        if (gameObject.transform.IsChildOf(WeaponSwitching.Instance.gameObject.transform))
        {
            foreach (var shotInfo in shotControllerShowCase.activeShotCtrl.m_shotList)
            {
                shotInfo.m_shotObj.GetComponent<UbhBaseShot>().m_angle = angle;
            }
        }
    }

    IEnumerator reload()
    {
        isReloading = true;
        if (isPlayer)
        {
            print("Reloading");
        }
        gunSounds.PlayOneShot(gunReloadSound); //gunsound for starting reload
        yield return new WaitForSeconds(GunProperties.reloadTime);
        //When you have a new gunsound for reload finished, put it here
        currentAmmo = GunProperties.maxAmmo;
        if (isPlayer)
        {
            PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
        }
        isReloading = false;
    }
    public int getCurrentAmmo()
    {
        return currentAmmo;
    }
    public void setCurrentAmmo(int setAmmo)
    {
        currentAmmo = setAmmo;
    }
}