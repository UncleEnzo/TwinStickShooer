using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowExplosive : Weapon
{
    public float throwForce = 100f;
    protected bool isPlayer = true;
    protected float lastThrown;
    public int currentAmmo = 1;
    public GameObject bullet;
    protected AudioSource gunSounds;
    public AudioClip gunShotSound;
    public AudioClip gunReloadSound;
    protected void Start()
    {
        gunSounds = GetComponent<AudioSource>();
        PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.playerUsable)
        {
            Vector3 mousePosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Transform playerTransform = Player.Instance.transform;
            Aim(mousePosTarget, playerTransform.position);
            SpriteFlip(playerTransform, mousePosTarget);
        }
        if (currentAmmo <= 0)
        {
            Destroy(gameObject);
        }
        if (Player.Instance.playerUsable)
        {
            Throw(isPlayer);
        }
        //Moving this outside of throw so that it is updated during timescale 0 as well
        PlayerHUBController.Instance.updateDisplayHubAmmo(currentAmmo);
    }

    protected void Throw(bool isPlayer)
    {
        if (Input.GetMouseButton(0) && (Time.time - lastThrown) > (1 / GunProperties.bulletsPerSecond))
        {
            lastThrown = Time.time;
            foreach (Transform thrownExplosive in GunProperties.bulletSpawnPoint)
            {
                GameObject newExplosive = Instantiate(bullet, thrownExplosive.position, thrownExplosive.rotation);
                Rigidbody2D rb = newExplosive.GetComponent<Rigidbody2D>();
                rb.velocity = (transform.right * throwForce);
                newExplosive.GetComponent<Explosive>().SetExplosiveProperties(GunProperties.explosionDamage, GunProperties.explosiveRadius, GunProperties.explosiveForce);
            }
            gunSounds.PlayOneShot(gunShotSound);
            currentAmmo--;
        }
    }
}
