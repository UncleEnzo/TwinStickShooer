using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowExplosive : Weapon
{
    protected bool isPlayer = true;
    protected float lastThrown;
    public int currentAmmo = 1;
    protected AudioSource gunSounds;
    public Sprite explosiveUIImage;
    public AudioClip gunShotSound;
    public AudioClip gunReloadSound;
    protected void Start()
    {
        gunSounds = GetComponent<AudioSource>();
        PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
        shotControllerShowCase = GetComponentInChildren<UbhShowcaseCtrl>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Player.Instance.playerUsable)
        {
            Vector3 mousePosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Transform playerTransform = Player.Instance.transform;
            Aim(mousePosTarget, playerTransform.position);
            SpriteFlip(playerTransform, mousePosTarget);
        }
        if (currentAmmo <= 0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(gameObject);
            }
            else
            {
                return;
            }
        }
        if (Player.Instance.playerUsable)
        {
            if (Input.GetMouseButton(0) && (Time.time - lastThrown) > (1 / GunProperties.bulletsPerSecond))
            {
                lastThrown = Time.time;
                bool isShooting = Throw(isPlayer);
                if (!isShooting)
                {
                    gunSounds.PlayOneShot(gunShotSound);
                    currentAmmo--;
                    PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
                }
            }
        }
    }

    private bool Throw(bool isPlayer)
    {
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
}

