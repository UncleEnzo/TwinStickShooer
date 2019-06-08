using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunProperties : MonoBehaviour
{
    //Gun Properties
    public WeaponType weaponType; //Defines the type of weapon this is
    public Transform[] bulletSpawnPoint; //Array so can fire multiple bullets at once
    [Range(.1f, 10f)] public float camShakeMagnitude = 1.5f;
    [Range(.01f, .1f)] public float camShakeLength = .05f;
    public float bulletsPerSecond = 5;
    public int maxAmmo = 10;
    public float reloadTime = 2f;

    //bullet properties
    public float bulletSpeed = 15f;
    public float bulletDamage = 1f;
    public float timeBulletSelfDestruct = 3f;
    public float knockBack = 300f;
    public float bulletAccuracy = 0f; //0 = perfect accuracy
    public float bulletAngle = 0f;
}
public enum WeaponType { Pistol, Shotgun, AssaultRifle, SniperRifle, MachineGun, RocketLaucher, RailGun, Knife }