using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Recipe : Interactable
{
    public Item item;
    public Signal recipePicked;
    public bool isFromChest = false;
    public int chestID = -1;
    private int compareChestID = -1;

    void Start()
    {
        GetComponent<Outline>().enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown("e") && playerInRange)
        {
            pickUpItem();
            sendDestroyChestSiblingsSignal();
        }
    }
    private void sendDestroyChestSiblingsSignal()
    {
        foreach (Recipe item in FindObjectsOfType<Recipe>())
        {
            item.SendMessage("CompareChestID", chestID);
        }
        recipePicked.Raise();
    }

    public void CompareChestID(int chestID)
    {
        compareChestID = chestID;
    }

    public void RecipePicked()
    {
        if (compareChestID != -1 && chestID != -1)
        {
            if (isFromChest && compareChestID == chestID)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Chest ID or Compare ChestID is an invalid value");
        }

    }

    void pickUpItem()
    {
        bool wasPickedUp = Inventory.instance.AddItem(item);
        if (wasPickedUp)
        {
            gameObject.SetActive(false);
        }
    }

    public override void interact()
    {
        base.interact();
        if (playerInRange)
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
