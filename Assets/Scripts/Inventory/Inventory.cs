using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion
    public Transform itemsParent;
    private GameObject recipeIconPanel;
    public GameObject recipeIcon;
    private List<Item> recipes = new List<Item>();
    public List<GameObject> recipeIcons = new List<GameObject>();
    public Item healingPotion;
    private GameObject keyIconPanel;
    public GameObject keyIcon;
    private GameObject keyIconCache;
    public Item key;
    private List<Item> keys = new List<Item>();
    public Item physicalComponent;
    private GameObject physicalSlot;
    [System.NonSerialized]
    public List<Item> physicalCraftComponents = new List<Item>();
    public Item gunpowderCompontent;
    private GameObject gunpowderSlot;
    [System.NonSerialized]
    public List<Item> gunpowderCraftComponents = new List<Item>();
    private GameObject explosiveSlot;

    public Item explosiveComponent;
    [System.NonSerialized]
    public List<Item> explosiveCraftComponents = new List<Item>();
    public Item moneyCoin;
    private List<Item> money = new List<Item>();
    private GameObject moneyIconPanel;
    public GameObject moneyIcon;
    private GameObject moneyIconCache;
    public int getKeyCount()
    {
        return keys.Count();
    }
    public int getPhysicalCount()
    {
        return physicalCraftComponents.Count();
    }
    public int getGunpowderCount()
    {
        return gunpowderCraftComponents.Count();
    }
    public int getExplosiveCount()
    {
        return explosiveCraftComponents.Count();
    }
    public int getMoneyCount()
    {
        return money.Count();
    }
    public List<Item> getRecipes()
    {
        return recipes;
    }
    public void Start()
    {
        recipeIconPanel = GameObject.Find("Canvas").transform.Find("RecipePanel").gameObject;
        keyIconPanel = GameObject.Find("Canvas").transform.Find("KeyPanel").gameObject;
        moneyIconPanel = GameObject.Find("Canvas").transform.Find("MoneyPanel").gameObject;
        foreach (Transform slot in itemsParent)
        {
            slot.GetComponentInChildren<Text>().text = "";
            if (slot.GetComponent<InventorySlot>().slotType == ItemType.Physical)
            {
                physicalSlot = slot.gameObject;
            }
            if (slot.GetComponent<InventorySlot>().slotType == ItemType.GunPowder)
            {
                gunpowderSlot = slot.gameObject;
            }
            if (slot.GetComponent<InventorySlot>().slotType == ItemType.Explosive)
            {
                explosiveSlot = slot.gameObject;
            }
        }

        if (SceneManager.GetActiveScene().buildIndex != SceneLoader.hubWorldIndex)
        {
            //Load in Health Recipe immediately 
            AddItem(healingPotion);

            //Load inventory from save data
            SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
            if (keys.Count() != SavePersistentData.keys)
            {
                LoadInventory(SavePersistentData.keys, key);
            }
            if (physicalCraftComponents.Count() != SavePersistentData.physicalCraftComponents)
            {
                LoadInventory(SavePersistentData.physicalCraftComponents, physicalComponent);
            }
            if (gunpowderCraftComponents.Count() != SavePersistentData.gunpowderCraftComponents)
            {
                LoadInventory(SavePersistentData.gunpowderCraftComponents, gunpowderCompontent);
            }
            if (explosiveCraftComponents.Count() != SavePersistentData.explosiveCraftComponents)
            {
                LoadInventory(SavePersistentData.explosiveCraftComponents, explosiveComponent);
            }
            if (recipes.Count() != SavePersistentData.acquiredRecipes.Count)
            {
                foreach (string item in SavePersistentData.acquiredRecipes)
                {
                    AddItem(LootLedger.LootLedgerDict[item].item.GetComponent<RecipePickUp>().item);
                }
            }
            SaveGlobalMoney SaveGlobalMoney = SaveSystem.LoadMoneyData();
            if (money.Count() != SaveGlobalMoney.money)
            {
                LoadInventory(SaveGlobalMoney.money, moneyCoin);
            }
        }
    }

    private void LoadInventory(int numOfItem, Item item)
    {
        for (int i = 0; i < numOfItem; i++)
        {
            Inventory.Instance.AddItem(item);
        }
    }

    public void AddItem(Item item)
    {
        //If the item is a key add it to the top left of the screen
        if (item.itemType == ItemType.Key)
        {
            if (keys.Count <= 0)
            {
                keyIconCache = Instantiate<GameObject>(keyIcon);
                GameObject keySprite = keyIconCache.transform.GetChild(0).gameObject;
                Text keyIconText = keyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                keyIconText.text = "";
                keySprite.GetComponent<Image>().sprite = item.icon;
                keys.Add(item);
                keyIconCache.transform.SetParent(keyIconPanel.transform);
                keyIconCache.SetActive(true);
            }
            else
            {
                keys.Add(item);
                Text keyIconText = keyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                keyIconText.text = "x" + keys.Count;
            }
        }
        //If the item is a coin add it to the top left of the screen
        else if (item.itemType == ItemType.Coin)
        {
            if (money.Count <= 0)
            {
                moneyIconCache = Instantiate<GameObject>(moneyIcon);
                GameObject moneySprite = moneyIconCache.transform.GetChild(0).gameObject;
                Text moneyIconText = moneyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                moneyIconText.text = "";
                moneySprite.GetComponent<Image>().sprite = item.icon;
                money.Add(item);
                moneyIconCache.transform.SetParent(moneyIconPanel.transform);
                moneyIconCache.SetActive(true);
            }
            else
            {
                money.Add(item);
                Text moneyIconText = moneyIconCache.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
                moneyIconText.text = "x" + money.Count;
            }
        }
        //if Item is recipe add to bottom of screen
        else if (item.itemType == ItemType.Recipe)
        {
            if (!recipes.Contains(item))
            {
                GameObject icon = Instantiate<GameObject>(recipeIcon);
                icon.transform.GetChild(1).GetComponent<Button>().interactable = false;
                recipeIcons.Add(icon);
                GameObject recipeSprite = icon.transform.GetChild(1).GetChild(0).gameObject;
                recipeSprite.GetComponent<Image>().sprite = item.icon;
                recipes.Add(item);
                icon.transform.SetParent(recipeIconPanel.transform);
                icon.SetActive(true);
                icon.GetComponent<TriggerRecipe>().item = item;
            }
        }
        else if (item.itemType == ItemType.Physical)
        {
            AddCraftComponentToUI(physicalCraftComponents, item, physicalSlot);
        }
        else if (item.itemType == ItemType.GunPowder)
        {
            AddCraftComponentToUI(gunpowderCraftComponents, item, gunpowderSlot);

        }
        else if (item.itemType == ItemType.Explosive)
        {
            AddCraftComponentToUI(explosiveCraftComponents, item, explosiveSlot);
        }
    }
    private void AddCraftComponentToUI(List<Item> itemList, Item item, GameObject slot)
    {
        if (itemList.Count <= 0)
        {
            slot.GetComponentInChildren<Image>().sprite = item.icon;
            slot.GetComponentInChildren<Image>().enabled = true;
            slot.GetComponentInChildren<Text>().text = "";
            itemList.Add(item);
        }
        else
        {
            itemList.Add(item);
            slot.GetComponentInChildren<Text>().text = itemList.Count.ToString();
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
        else if (item.itemType == ItemType.Physical)
        {
            RemoveCraftComponentFromUI(physicalCraftComponents, item, physicalSlot);
        }
        else if (item.itemType == ItemType.GunPowder)
        {
            RemoveCraftComponentFromUI(gunpowderCraftComponents, item, gunpowderSlot);
        }
        else if (item.itemType == ItemType.Explosive)
        {
            RemoveCraftComponentFromUI(explosiveCraftComponents, item, explosiveSlot);
        }

    }
    private void RemoveCraftComponentFromUI(List<Item> itemList, Item item, GameObject slot)
    {
        itemList.Remove(item);
        if (itemList.Count > 0)
        {
            slot.GetComponentInChildren<Text>().text = itemList.Count.ToString();
        }
        else
        {
            slot.GetComponentInChildren<Image>().enabled = false;
            slot.GetComponentInChildren<Text>().text = "";
        }
    }
}