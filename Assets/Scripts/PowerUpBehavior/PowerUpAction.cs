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

    public void HealStartAction()
    {
        Debug.Log("Triggered HP Up");
        Player.localPlayerData.health += healingIncrease;
        PlayerHUBController.Instance.updateDisplayHubHealth(Player.localPlayerData.health);
    }
    public void HealEndAction()
    {
        Debug.Log("End HP Up. Decriment stack count");
    }

    float speedUpIncrease = 5f;
    int speedUpStackCount = 0;

    public void HighSpeedStartAction()
    {
        Debug.Log("Triggered HighSpeedRecipe");
        speedUpStackCount += 1;
        Player.Instance.speed += speedUpIncrease;
    }

    public void HighSpeedEndAction()
    {
        Debug.Log("HighSpeedRecipe Expired");
        float speedReduction = speedUpIncrease * speedUpStackCount;
        Player.Instance.speed -= speedReduction;
        speedUpStackCount = 0;
    }
    float colliderSizeIncrease = .2f;
    //note, need to move the offset as well or it'll get bulkier both ways
    float parryCoolDecrease = 1f;
    int parryStackCount = 0;

    public void ParryStartAction()
    {
        Debug.Log("Triggered Parry");
        FindObjectOfType<Player>().GetComponent<Parry>().enabled = true;
        parryStackCount += 1;
        if (parryStackCount >= 2)
        {
            FindObjectOfType<Player>().GetComponent<Parry>().colliderSizeX += colliderSizeIncrease;
            FindObjectOfType<Player>().GetComponent<Parry>().coolDownResetValue -= parryCoolDecrease;
        }
    }

    public void ParryEndAction()
    {
        Debug.Log("Parry Expired");
        parryStackCount = 0;
        FindObjectOfType<Player>().GetComponent<Parry>().enabled = false;
    }
    float bulletSpeedIncrease = 5f;
    int bulletSpeedStackCount = 0;

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
