using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorInventory : MonoBehaviour
{
    public string VendorName;
    public List<Loot> Inventory = new List<Loot>();
    public GameObject weeklyItemPopup;
    private Loot itemForSale;

    void Start()
    {
        //Load inventory from vendor dict
        Inventory.Clear();
        SaveVendorLootPool SaveVendorLootPool = SaveSystem.LoadVendorLootPoolData();
        foreach (KeyValuePair<string, List<string>> entry in SaveVendorLootPool.VendorLootPool)
        {
            if (entry.Key == VendorName)
            {
                foreach (string objName in entry.Value)
                {
                    Inventory.Add(LootLedger.LootLedgerDict[objName]);
                }
                break;
            }
        }
        selectItemForSale();
    }

    void Update()
    {
        //Note: If player buys the item for sale, need to add logic that makes selects a new item
        if (itemForSale == null)
        {
            selectItemForSale(); // I think this is a janky solution because if you run out of items, this will constantly call null
        }
    }

    //Note: This is just a basic way to select the item. Later this method needs to be revised
    private void selectItemForSale()
    {
        itemForSale = Inventory[0];
    }

    //when player is in range, have him show the item he's holding this week
    //if player buys it, need to offer another item
    //need to wait for interation
    //Need to offer player the item that he's selling to examine
    //Need to have a transaction working

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        //Vendor raises signal showing what Item he is carrying
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            //This is still buggy, needs work. Item icon needs to be smaller too and have set dimensions. 
            //Should also be the recipe icon, not the sprite of the scroll
            //Weapon icon should be flipped in the correct direction
            weeklyItemPopup.GetComponentInChildren<SpriteRenderer>().sprite = itemForSale.item.GetComponent<SpriteRenderer>().sprite;
            weeklyItemPopup.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        //Vendor stops showing what Item he is carrying
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            weeklyItemPopup.SetActive(false);
        }
    }
}
