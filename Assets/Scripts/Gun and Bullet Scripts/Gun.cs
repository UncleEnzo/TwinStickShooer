using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : Weapon
{
    protected bool isPlayer;
    protected float lastfired;
    protected bool isReloading = false;
    protected int currentAmmo;
    protected GameObject player;
    //Properties for the gun and bullet

    public GameObject bullet;
    protected AudioSource gunSounds;
    public AudioClip gunShotSound;
    public AudioClip gunReloadSound;
    private UbhShowcaseCtrl shotControllerShowCase;
    protected Vector3 mousePosTarget;
    protected Transform playerTransform;

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
            // foreach (Transform bulletShot in GunProperties.bulletSpawnPoint)
            // {
            shotControllerShowCase.activeShotCtrl.StartShotRoutine();
            // }
            gunSounds.PlayOneShot(gunShotSound);
            currentAmmo--;
            PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
            CameraController.Instance.Shake((player.transform.position - transform.position).normalized, GunProperties.camShakeMagnitude, GunProperties.camShakeLength);
        }
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

        //  MOVE TO CHEATS, USE LATER FOR RECIPES THAT CHANGE UP FIRE PATTERNS :D
        //Next Shot
        if (Input.GetKeyDown("9"))
        {
            shotControllerShowCase.ChangeShot(true);
        }
        //previous shot
        if (Input.GetKeyDown("0"))
        {
            shotControllerShowCase.ChangeShot(false);
        }

    }

    private void FireEnemyGun()
    {
        if ((Time.time - lastfired) > (1 / GunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            //note: does not need bullet angle because it always has lock-on set to player
            //Note: There may be some enemies that fire weird patterns that require manual aim
            shotControllerShowCase.activeShotCtrl.StartShotRoutine();
            gunSounds.PlayOneShot(gunShotSound);
            currentAmmo--;
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
}