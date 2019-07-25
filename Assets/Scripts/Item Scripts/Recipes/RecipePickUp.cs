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
    public string DamageDescription = "5% damage increase";
    public string EffectDescription = "Does stuff";
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
        RecipeUIPanel.SetActive(false);
        pickUpItem();
        sendDestroyChestSiblingsSignal();
        Player.Instance.enablePlayer(true);
        LootTable.instance.RemoveItemFromPool(gameObject);
        Time.timeScale = 1;

        //Removes listeners from the UI
        for (int i = 1; i < 4; i++)
        {
            RemoveButtonListeners(RecipeUIPanel, i);
        }
    }

    private void RemoveButtonListeners(GameObject RecipeUIPanel, int RecipePanel)
    {
        GameObject Panel = RecipeUIPanel.transform.GetChild(RecipePanel).gameObject;
        Button Button = Panel.transform.GetChild(3).GetComponent<Button>();
        Button.onClick.RemoveAllListeners();
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
        Inventory.Instance.AddItem(item);
        gameObject.SetActive(false);
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
