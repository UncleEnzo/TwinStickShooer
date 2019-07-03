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
    protected void Start()
    {
        gunSounds = GetComponent<AudioSource>();
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
            foreach (Transform bulletShot in GunProperties.bulletSpawnPoint)
            {
                GameObject newBullet = ObjectPooler.SharedInstance.GetPooledObject(bullet.name + "(Clone)");
                if (newBullet != null)
                {
                    newBullet.transform.position = bulletShot.position;
                    newBullet.transform.rotation = bulletShot.rotation;
                    newBullet.SetActive(true);
                }
                newBullet.GetComponent<PlayerBullet>().setPlayerBulletProperties(GunProperties.bulletSpeed, GunProperties.bulletDamage, GunProperties.timeBulletSelfDestruct, GunProperties.knockBack, GunProperties.bulletAccuracy, GunProperties.bulletAngle, GunProperties.bulletBounce);
            }
            gunSounds.PlayOneShot(gunShotSound);
            currentAmmo--;
            PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
            CameraController.Instance.Shake((player.transform.position - transform.position).normalized, GunProperties.camShakeMagnitude, GunProperties.camShakeLength);
        }
    }

    private void FireEnemyGun()
    {
        if ((Time.time - lastfired) > (1 / GunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            foreach (Transform bulletShot in GunProperties.bulletSpawnPoint)
            {
                GameObject newBullet = ObjectPooler.SharedInstance.GetPooledObject(bullet.name + "(Clone)");
                if (newBullet != null)
                {
                    newBullet.transform.position = bulletShot.position;
                    newBullet.transform.rotation = bulletShot.rotation;
                    newBullet.SetActive(true);
                }
            }
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