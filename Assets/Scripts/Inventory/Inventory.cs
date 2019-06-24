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
    private GameObject keyIconPanel;
    public GameObject keyIcon;
    private GameObject keyIconCache;
    private List<Item> keys = new List<Item>();
    private GameObject moneyIconPanel;
    public GameObject moneyIcon;
    private GameObject moneyIconCache;
    private List<Item> money = new List<Item>();

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
        keyIconPanel = GameObject.Find("Canvas").transform.Find("KeyPanel").gameObject;
        moneyIconPanel = GameObject.Find("Canvas").transform.Find("MoneyPanel").gameObject;
    }
    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        //If the item is a key add it to the top left of the screen
        if (e.Item.itemType == ItemType.Key)
        {
            if (keys.Count <= 0)
            {
                keyIconCache = Instantiate<GameObject>(keyIcon);
                GameObject keySprite = keyIconCache.transform.GetChild(0).gameObject;
                Text keyIconText = keyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                keyIconText.text = "";
                keySprite.GetComponent<Image>().sprite = e.Item.icon;
                keys.Add(e.Item);
                keyIconCache.transform.SetParent(keyIconPanel.transform);
                keyIconCache.SetActive(true);
            }
            else
            {
                keys.Add(e.Item);
                Text keyIconText = keyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                keyIconText.text = "x" + keys.Count;
            }
        }
        //If the item is a coin add it to the top left of the screen
        else if (e.Item.itemType == ItemType.Coin)
        {
            if (money.Count <= 0)
            {
                moneyIconCache = Instantiate<GameObject>(moneyIcon);
                GameObject moneySprite = moneyIconCache.transform.GetChild(0).gameObject;
                Text moneyIconText = moneyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                moneyIconText.text = "";
                print(moneyIconText.text);
                moneySprite.GetComponent<Image>().sprite = e.Item.icon;
                money.Add(e.Item);
                moneyIconCache.transform.SetParent(moneyIconPanel.transform);
                moneyIconCache.SetActive(true);
            }
            else
            {
                money.Add(e.Item);
                Text moneyIconText = moneyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                moneyIconText.text = "x" + money.Count;
            }
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
        else if (item.itemType == ItemType.Coin)
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
        if (item.itemType == ItemType.Key)
        {
            keys.Remove(item);
            if (keys.Count > 0)
            {
                Text keyIconText = keyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                keyIconText.text = "x" + keys.Count;
            }
            else
            {
                keyIconCache.SetActive(false);
                Destroy(keyIconCache);
            }
        }

        else if (item.itemType == ItemType.Coin)
        {
            money.Remove(item);
            if (money.Count > 0)
            {
                Text moneyIconText = moneyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                moneyIconText.text = "x" + money.Count;
            }
            else
            {
                moneyIconCache.SetActive(false);
                Destroy(moneyIconCache);
            }
        }
        else
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
}