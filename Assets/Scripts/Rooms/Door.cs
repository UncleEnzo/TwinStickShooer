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

    //need to refine this so it only works when player enters then exits through the other side
    void OnTriggerExit2D(Collider2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "Player" && thisDoorType == DoorType.enemy)
        {
            Close();
            //activate enemyspawner
            //populate enemy room script with enemies
            //start enemyRoom
        }
    }
    public void Open()
    {
        doorSprite.enabled = false;
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