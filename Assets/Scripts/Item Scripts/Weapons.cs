using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPickup", menuName = "Inventory/WeaponPickup")]
public class Weapons : Item
{
    public WeaponSlot weaponSlot;
}
public enum WeaponSlot { Pistol, Shotgun, AssaultRifle, MachineGun, RocketLaucher, RailGun }
