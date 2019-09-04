using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : Weapon
{
    private int currentAmmo;
    protected bool isPlayer;
    protected float lastfired;
    protected bool isReloading = false;
    private Slider reloadSlider;
    private GameObject reloadUIObject;
    Coroutine lastCoroutine = null;
    protected GameObject player;
    [Header("Sound effects")]
    protected AudioSource gunSounds;
    public AudioClip gunShotSound;
    public AudioClip gunReloadSound;

    protected void Start()
    {
        reloadUIObject = GameObject.Find("Canvas").transform.Find("ReloadSlider").gameObject;
        reloadSlider = reloadUIObject.GetComponent<Slider>();
        gunSounds = GetComponent<AudioSource>();
        shotControllerShowCase = GetComponentInChildren<UbhShowcaseCtrl>();
        currentAmmo = GunProperties.maxAmmo;
        player = Player.Instance.transform.gameObject;
        PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
    }

    private void OnEnable()
    {
        if (reloadUIObject != null)
        {
            reloadUIObject.SetActive(false);
        }
        isReloading = false;
    }

    private void OnDisable()
    {
        isReloading = false;
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
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
                lastCoroutine = StartCoroutine(reload());
                return;
            }
        }
        if (currentAmmo <= 0)
        {
            lastCoroutine = StartCoroutine(reload());
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

    new protected void Update()
    {
        if (isReloading)
        {
            reloadSlider.value += Time.deltaTime;
        }
        base.Update();
    }

    IEnumerator reload()
    {
        if (isPlayer)
        {
            reloadUIObject.SetActive(true);
            reloadSlider.value = 0;
            reloadSlider.maxValue = GunProperties.reloadTime;
        }
        isReloading = true;
        gunSounds.PlayOneShot(gunReloadSound); //gunsound for starting reload
        yield return new WaitForSeconds(GunProperties.reloadTime);
        //When you have a new gunsound for reload finished, put it here
        reloadUIObject.SetActive(false);
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