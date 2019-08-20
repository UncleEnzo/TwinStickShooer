using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagsAndLabels
{
    [Header("Potion Names")]
    public const string HealthPotionRecipe = "HealthPotionRecipe";

    [Header("Tags")]
    public const string UntaggedTag = "Untagged";
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public const string InteractableTag = "Interactable";
    public const string PickUpTag = "PickUp";
    public const string WallTag = "Wall";
    public const string GroundTag = "Ground";
    public const string HealthUITag = "HealthUI";
    public const string GunUITag = "GunUI";
    public const string AmmoUITag = "AmmoUI";

    [Header("Vendor name string. (Not tags or labels)")]
    public const string PhysicalVendor = "PhysicalVendor";
    public const string GunpowderVendor = "GunpowderVendor";
    public const string ExplosiveVendor = "ExplosiveVendor";
    public const string WeaponsVendor = "WeaponsVendor";

    [Header("Labels")]
    public const string DoorLabel = "Door";
    public const string ChestLabel = "Chest";
    public const string DeadEnemyLabel = "DeadEnemy";
    public const string EnemyLabel = "Enemy";
    public const string PlayerLabel = "Player";
    public const string PlayerBulletLabel = "PlayerBullet";
    public const string EnemyBulletLabel = "EnemyBullet";
    public const string NPCLabel = "NPC";
    public const string DefaultLabel = "Default";
}
