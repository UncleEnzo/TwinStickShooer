using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
public enum WeaponType { Pistol, Shotgun, AssaultRifle, MachineGun, RocketLaucher, RailGun, Knife }