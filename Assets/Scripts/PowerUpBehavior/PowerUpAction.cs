using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpAction : MonoBehaviour
{
    //Needs to calculate the amount of times it is called so that when it ends,
    //the end action can cancel the amount of a recipe was used
    //without disabling the effects of other recipes
    float healingIncrease = 1f;
    float speedUpIncrease = 5f;
    private static int speedUpStackCount;
    private float colliderSizeIncrease = .3f;
    private float parryCoolDecrease = .5f;
    private static int parryStackCount;
    float bulletSpeedIncrease = 5f;
    private static int bulletSpeedStackCount;
    int bulletBounceIncrease = 2;
    float ExplosiveDamageIncrease = 1f;
    float ExplosiveRadiusIncrease = 1f;
    float ExplosiveForceIncrease = 1f;
    private static int ExplosiveBulletStackCount;

    public void Start()
    {
        //resets stack counts
        speedUpStackCount = 0;
        parryStackCount = 0;
        bulletSpeedStackCount = 0;
        ExplosiveBulletStackCount = 0;
    }

    public void HealStartAction()
    {
        Debug.Log("Triggered HP Up");
        Player.Instance.health += healingIncrease;
        PlayerHUBController.Instance.updateDisplayHubHealth(Player.Instance.health, Player.Instance.totalHealth);
    }
    public void HealEndAction()
    {
        Debug.Log("End HP Up. Decriment stack count");
    }

    public void HighSpeedStartAction()
    {
        Debug.Log("Triggered HighSpeedRecipe");
        speedUpStackCount += 1;
        print(speedUpStackCount);
        Player.Instance.speed += speedUpIncrease;
    }

    public void HighSpeedEndAction()
    {
        Debug.Log("HighSpeedRecipe Expired");
        float speedReduction = speedUpIncrease * speedUpStackCount;
        Player.Instance.speed -= speedReduction;
        speedUpStackCount = 0;
    }
    public void ParryStartAction()
    {
        Debug.Log("Triggered Parry");
        Parry parry = WeaponSwitching.Instance.GetComponent<Parry>();
        WeaponSwitching.Instance.GetComponent<Parry>().enabled = true;
        parryStackCount += 1;
        if (parryStackCount >= 2)
        {
            parry.colliderSizeX += colliderSizeIncrease;
            parry.coolDownResetValue -= parryCoolDecrease;
            parry.updateBoxCollider();
        }
    }

    public void ParryEndAction()
    {
        Debug.Log("Parry Expired");
        parryStackCount = 0;
        print(parryStackCount);
        WeaponSwitching.Instance.GetComponent<Parry>().enabled = false;
    }

    public void BulletSpeedStartAction()
    {
        Debug.Log("Triggered Bullet Speed Recipe");
        bulletSpeedStackCount += 1;
        PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
        List<GunProperties> gunProperties = new List<GunProperties>();
        foreach (PlayerGun gun in playerGuns)
        {
            gunProperties.Add(gun.GunProperties);
        }
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.bulletSpeed += bulletSpeedIncrease;
        }
    }
    public void BulletSpeedEndAction()
    {
        Debug.Log("Bullet Speed Recipe Expired");
        float bulletSpeedReduction = bulletSpeedIncrease * bulletSpeedStackCount;
        PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
        List<GunProperties> gunProperties = new List<GunProperties>();
        foreach (PlayerGun gun in playerGuns)
        {
            gunProperties.Add(gun.GunProperties);
        }
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.bulletSpeed -= bulletSpeedReduction;
        }
        bulletSpeedStackCount = 0;
    }

    public void BulletBounceStartAction()
    {
        Debug.Log("Triggered Bouncy Bullet Recipe");
        PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
        List<GunProperties> gunProperties = new List<GunProperties>();
        foreach (PlayerGun gun in playerGuns)
        {
            gunProperties.Add(gun.GunProperties);
        }
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.isBulletBounce = true;
            gunProperty.bulletBounceMaxNum += bulletBounceIncrease;
        }
    }
    public void BulletBounceEndAction()
    {
        Debug.Log("Bouncy Bullet Recipe Expired");
        PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
        List<GunProperties> gunProperties = new List<GunProperties>();
        foreach (PlayerGun gun in playerGuns)
        {
            gunProperties.Add(gun.GunProperties);
        }
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.bulletBounceMaxNum = 0;
            gunProperty.isBulletBounce = false;
        }
    }
    public void ExplosiveBulletStartAction()
    {
        Debug.Log("Triggered Bullet Explosion Recipe");
        if (ExplosiveBulletStackCount == 0)
        {
            PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
            List<GunProperties> gunProperties = new List<GunProperties>();
            foreach (PlayerGun gun in playerGuns)
            {
                gunProperties.Add(gun.GunProperties);
            }
            foreach (GunProperties gunProperty in gunProperties)
            {
                gunProperty.isExplosive = true;
            }
        }
        else
        {
            PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
            List<GunProperties> gunProperties = new List<GunProperties>();
            foreach (PlayerGun gun in playerGuns)
            {
                gunProperties.Add(gun.GunProperties);
            }
            foreach (GunProperties gunProperty in gunProperties)
            {
                gunProperty.explosionDamage += ExplosiveDamageIncrease;
                gunProperty.explosiveRadius += ExplosiveRadiusIncrease;
                gunProperty.explosiveForce += ExplosiveForceIncrease;
            }
        }
        ExplosiveBulletStackCount += 1;
    }
    public void ExplosiveBulletEndAction()
    {
        Debug.Log("Bullet Explosion Recipe Expired");
        float ExplosiveDamageReduction = ExplosiveDamageIncrease * ExplosiveBulletStackCount;
        float ExplosiveRadiusReduction = ExplosiveRadiusIncrease * ExplosiveBulletStackCount;
        float ExplosiveForceReduction = ExplosiveForceIncrease * ExplosiveBulletStackCount;
        PlayerGun[] playerGuns = FindObjectsOfType<PlayerGun>();
        List<GunProperties> gunProperties = new List<GunProperties>();
        foreach (PlayerGun gun in playerGuns)
        {
            gunProperties.Add(gun.GunProperties);
        }
        foreach (GunProperties gunProperty in gunProperties)
        {
            gunProperty.isExplosive = false;
            gunProperty.explosionDamage -= ExplosiveDamageReduction;
            gunProperty.explosiveRadius -= ExplosiveRadiusReduction;
            gunProperty.explosiveForce -= ExplosiveForceReduction;
        }
        ExplosiveBulletStackCount = 0;
    }
    public GameObject starShatter;
    public void StarShatterStartAction()
    {
        Debug.Log("Triggered Star Shatter Recipe");
        AddOrStackExplosive(starShatter);
    }
    public void StarShatterEndAction()
    {
        Debug.Log("Star Shatter added to Weapon Holder");
    }
    private void AddOrStackExplosive(GameObject explosivePrefab)
    {
        bool playerHasExplosive = false;
        foreach (Transform weapon in FindObjectOfType<SetGunPosition>().transform)
        {
            if (weapon.GetComponent<Weapon>().GunProperties.weaponType == explosivePrefab.GetComponent<Weapon>().GunProperties.weaponType)
            {
                playerHasExplosive = true;
                weapon.GetComponent<ThrowExplosive>().currentAmmo++;
                PlayerHUBController.Instance.updateDisplayHubAmmo(weapon.GetComponent<ThrowExplosive>().currentAmmo);
            }
        }
        if (playerHasExplosive == false)
        {
            GameObject Explosive = Instantiate(explosivePrefab, FindObjectOfType<SetGunPosition>().transform.position, FindObjectOfType<SetGunPosition>().transform.rotation);
            Explosive.transform.SetParent(FindObjectOfType<SetGunPosition>().transform);
            WeaponSwitching.Instance.weaponCount = WeaponSwitching.Instance.transform.childCount;
            WeaponSwitching.Instance.autoSelectNewWeaponInHolster();
            Explosive.GetComponent<EquipToPlayer>().enabled = false;
            Explosive.GetComponent<Weapon>().enabled = false; // so that it's unusable until the player turns off tab
            Explosive.GetComponent<Collider2D>().enabled = false;
        }
    }
}
