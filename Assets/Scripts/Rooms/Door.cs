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
    public BoxCollider2D doorClosedCollider;
    public BoxCollider2D isTriggerCollider;
    public DoorType thisDoorType;
    public bool open = false;
    //public Inventory playerInventory; NOTE: For Key
    public SpriteRenderer doorSprite;
    // public BoxCollider2D physicsCollider;
    public Sprite openDoor;
    public Sprite closedEnemyDoor;
    public Sprite closedKeyDoor;
    public Signal KillDoorTriggered;

    private GameObject tileMapGameObject;

    void Start()
    {
        tileMapGameObject = this.transform.root.gameObject;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && open && thisDoorType == DoorType.enemy)
        {
            FindObjectOfType<EnemyRoom>().SendMessage("GetTileMapData", tileMapGameObject);
            FindObjectOfType<EnemySpawner>().SendMessage("GetTileMapData", tileMapGameObject);
            KillDoorTriggered.Raise();
        }

        //Need to make sure you're not fighting enemies in a kill room also
        if (other.CompareTag("Player") && !other.isTrigger && Input.GetKeyDown("e") && thisDoorType == DoorType.key)
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
    public void Open()
    {
        if (thisDoorType == DoorType.key)
        {
            doorSprite.sprite = openDoor;
        }
        if (thisDoorType == DoorType.enemy)
        {
            doorSprite.sprite = openDoor;
        }
        open = true;
        doorClosedCollider.enabled = false;
    }

    public void Close()
    {
        open = false;
        doorClosedCollider.enabled = true;
        doorSprite.sprite = closedEnemyDoor;
    }
}