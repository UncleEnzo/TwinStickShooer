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
    public SpriteRenderer doorSpriteRenderer;
    // public BoxCollider2D physicsCollider;
    public Sprite openDoor;
    public Sprite closedDoor;
    public Item key;
    public Signal KillDoorTriggered;
    private GameObject tileMapGameObject;
    private GameObject groundGameObject;
    private bool otherColliderIsTrigger = false;
    private int remainingNumEnemies;

    void Start()
    {
        tileMapGameObject = this.transform.parent.parent.gameObject;
        foreach (Transform child in tileMapGameObject.transform)
        {
            if (child.tag == TagsAndLabels.GroundTag)
            {
                groundGameObject = child.gameObject;
            }
        }
    }

    void Update()
    {
        if (thisDoorType == DoorType.key && remainingNumEnemies == 0 && otherColliderIsTrigger == true && Input.GetKeyDown("e"))
        {
            if (Inventory.Instance.getKeyCount() > 0)
            {
                // if player has key, then open Insert when you make keys
                if (!open)
                {
                    Open();
                    Inventory.Instance.RemoveItem(key);
                }
                else
                {
                    Debug.Log("Key door open when it should be closed");
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        playerInRange = true;
        if (other.CompareTag(TagsAndLabels.PlayerTag) && !other.isTrigger && open && thisDoorType == DoorType.enemy)
        {
            EnemySpawner.Instance.GetGroundTileMapData(groundGameObject);
            EnemyRoom.Instance.GetRoomData(tileMapGameObject);
            KillDoorTriggered.Raise();
        }

        if (!other.isTrigger)
        {
            otherColliderIsTrigger = true;
            if (other.CompareTag(TagsAndLabels.PlayerTag))
            {
                remainingNumEnemies = EnemyRoom.Instance.numRemainingEnemies;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
        otherColliderIsTrigger = true;
        remainingNumEnemies = 1;
    }

    public void Open()
    {
        doorSpriteRenderer.sprite = openDoor;
        open = true;
        doorClosedCollider.enabled = false;
    }

    public void Close()
    {
        open = false;
        doorClosedCollider.enabled = true;
        doorSpriteRenderer.sprite = closedDoor;
    }
}