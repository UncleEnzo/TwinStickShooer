using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        instance = this;
    }
    #endregion
    public IList<InventorySlot> inventorySlots = new List<InventorySlot>();
    public Transform itemsParent;
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    private GameObject recipeIconPanel;
    public GameObject recipeIcon;
    private List<Item> recipes = new List<Item>();
    public List<GameObject> recipeIcons = new List<GameObject>();
    public int numOfKeys;

    public void Start()
    {
        ItemAdded += InventoryScript_ItemAdded;
        ItemRemoved += InventoryScript_ItemRemoved;

        int id = 0;
        foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
        {
            inventorySlots.Add(slot);
            slot.inventoryId = id;
            id++;
        }
        recipeIconPanel = GameObject.Find("Canvas").transform.Find("RecipePanel").gameObject;
    }
    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        //If the item is a key add it to the top left of the screen
        if (e.Item.itemType == ItemType.Key)
        {
            print("PICKED UP KEY, BUT DIDN'T ADD IT TO INVENTORY OR ANYTHING");
        }
        //if Item is recipe add to bottom of screen
        else if (e.Item.itemType == ItemType.Recipe)
        {
            if (!recipes.Contains(e.Item))
            {
                GameObject icon = Instantiate<GameObject>(recipeIcon);
                icon.transform.GetChild(1).GetComponent<Button>().interactable = false;
                recipeIcons.Add(icon);
                GameObject recipeSprite = icon.transform.GetChild(1).GetChild(0).gameObject;
                recipeSprite.GetComponent<Image>().sprite = e.Item.icon;
                recipes.Add(e.Item);
                icon.transform.SetParent(recipeIconPanel.transform);
                icon.SetActive(true);
                icon.GetComponent<TriggerRecipe>().item = e.Item;
            }
        }
        //Otherwise place it as a crafting component
        else
        {
            int index = -1;
            foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
            {
                index++;
                if (index == e.Item.inventorySlot.Id)
                {
                    slot.icon.sprite = e.Item.icon;
                    slot.icon.enabled = true;

                    int itemCount = e.Item.inventorySlot.Count;
                    if (itemCount > 1)
                    {
                        slot.txtCount.text = itemCount.ToString();
                    }
                    else
                    {
                        slot.txtCount.text = "";
                    }
                    break;
                }
            }
        }
    }
    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        int index = -1;
        foreach (InventorySlot slot in itemsParent.GetComponentsInChildren<InventorySlot>())
        {
            index++;
            if (index == e.Item.inventorySlot.Id)
            {
                int itemCount = e.Item.inventorySlot.Count;
                if (itemCount < 2)
                {
                    slot.txtCount.text = "";
                }
                else
                {
                    slot.txtCount.text = itemCount.ToString();
                }
                if (itemCount == 0)
                {
                    slot.icon.sprite = null;
                    slot.icon.enabled = false;
                }
            }
        }
    }
    private InventorySlot findStackableSlot(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.isStackable(item))
            {
                return slot;
            }
        }
        return null;
    }
    private InventorySlot findNextEmptySlot()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.isEmpty)
            {
                return slot;
            }
        }
        return null;
    }
    public bool AddItem(Item item)
    {
        if (item.itemType == ItemType.Key)
        {
            ItemAdded(this, new InventoryEventArgs(item));
            return true;
        }
        else if (item.itemType == ItemType.Recipe)
        {
            ItemAdded(this, new InventoryEventArgs(item));
            return true;
        }
        else
        {
            InventorySlot freeSlot = findStackableSlot(item);
            if (freeSlot == null)
            {
                freeSlot = findNextEmptySlot();
            }
            if (freeSlot != null)
            {
                freeSlot.AddItem(item);
                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
            return true;
        }
    }
    public void RemoveItem(Item item)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.Remove(item))
            {
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, new InventoryEventArgs(item));
                }
                break;
            }
        }
    }

}