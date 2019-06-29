using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.UI;

public class RecipePickUp : Interactable
{
    public Item item;
    public Signal recipePicked;
    public string damageDescription;
    public string effectDescription;
    public bool isFromChest = false;
    public int chestID = -1;
    private int compareChestID = -1;

    void Start()
    {
        GetComponent<cakeslice.Outline>().enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown("e") && playerInRange)
        {
            pickUpItem();
            sendDestroyChestSiblingsSignal();
        }
    }

    public void MakeSelection()
    {
        GameObject RecipeUIPanel = GameObject.Find("Canvas").transform.Find("RecipeSelectMenu").gameObject;

        //Testing for bug with NPE when multiple recipe chests...
        // GameObject physicalPanel = RecipeUIPanel.transform.GetChild(1).gameObject;
        // Button physicalButton = physicalPanel.transform.GetChild(3).GetComponent<Button>();
        // physicalButton.onClick.RemoveListener(() => MakeSelection());

        RecipeUIPanel.SetActive(false);
        pickUpItem();
        sendDestroyChestSiblingsSignal();
        Player.enablePlayer(true);
        Time.timeScale = 1;
    }

    private void sendDestroyChestSiblingsSignal()
    {
        foreach (RecipePickUp item in FindObjectsOfType<RecipePickUp>())
        {
            item.SendMessage("CompareChestID", chestID);
        }
        recipePicked.Raise();

        //calls the signal on itself as well to make sure all objects fomr the chest are deleted
        compareChestID = chestID;
        RecipePicked();
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
                print("Destroying gameobject: " + gameObject.name);
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
        bool wasPickedUp = Inventory.Instance.AddItem(item);
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
            GetComponent<cakeslice.Outline>().enabled = true;
        }
        else
        {
            GetComponent<cakeslice.Outline>().enabled = false;
        }
    }
}
