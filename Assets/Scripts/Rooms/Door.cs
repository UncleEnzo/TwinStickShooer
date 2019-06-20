using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum DoorType
{
    key, enemy, button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    //public Inventory playerInventory; NOTE: For Key
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public Sprite openDoor;
    public Sprite closedEnemyDoor;
    public Sprite closedKeyDoor;

    void Update()
    {
        if (Input.GetKeyDown("e") && thisDoorType == DoorType.key)
        {
            // if (playerInventory.numberOfKeys > 0){
            //if player has key, then open Insert when you make keys
            //playerInventory.numberOfKeys--;
            if (!open)
            {
                Open();
            }
            else
            {
                Close();
            }
            // }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && open)
        {
            FindObjectOfType<EnemySpawner>().spawnKillRoomRandomEnemies(5);
        }
    }
    public void Open()
    {
        if (thisDoorType == DoorType.key)
        {
            doorSprite.sprite = closedKeyDoor;
        }
        if (thisDoorType == DoorType.enemy)
        {
            doorSprite.sprite = closedEnemyDoor;
        }
        open = true;
        physicsCollider.enabled = false;
    }

    public void Close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
    }

}