using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VendorInventory : MonoBehaviour
{
    public VendorType vendorType;
    public List<Loot> vendorInventory = new List<Loot>();
    public GameObject weeklyItemPopup;
    private Boolean playerInRange = false;
    private Loot itemForSale;
    private GameObject TradePanel;
    private TextMeshProUGUI ItemNameText;
    private TextMeshProUGUI ItemCostText;
    private Image ItemIcon;
    private Button ItemBuyButton;
    private Button ItemExitButton;
    private Image ItemColor;
    private TextMeshProUGUI ItemDamageText;
    private TextMeshProUGUI ItemEffectText;

    void Start()
    {
        //Load inventory from vendor dict
        vendorInventory.Clear();
        SaveVendorLootPool SaveVendorLootPool = SaveSystem.LoadVendorLootPoolData();
        foreach (KeyValuePair<VendorType, List<string>> entry in SaveVendorLootPool.VendorLootPool)
        {
            if (entry.Key == vendorType)
            {
                foreach (string objName in entry.Value)
                {
                    vendorInventory.Add(LootLedger.LootLedgerDict[objName]);
                }
                break;
            }
        }
        selectItemForSale();

        //Prepare the Vendor Trade UI
        TradePanel = GameObject.Find("Canvas").transform.Find("VendorPurchaseMenu").gameObject;
        GetVendorTradePanelUIPanel();
        TradePanel.SetActive(false);
    }

    void Update()
    {
        //Note: If player buys the item for sale, need to add logic that makes selects a new item
        if (itemForSale == null)
        {
            selectItemForSale(); // I think this is a janky solution because if you run out of items, this will constantly call null
        }
        if (playerInRange && Input.GetKeyDown("e"))
        {
            print("Activating panel");
            Time.timeScale = 0;
            Player.Instance.enablePlayer(false);
            weeklyItemPopup.SetActive(false);
            updateVendorTradeUI();
            TradePanel.SetActive(true);
        }
    }

    private void GetVendorTradePanelUIPanel()
    {
        ItemNameText = TradePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ItemCostText = TradePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        ItemIcon = TradePanel.transform.GetChild(2).GetComponent<Image>();
        ItemBuyButton = TradePanel.transform.GetChild(3).GetChild(1).GetComponent<Button>();
        ItemExitButton = TradePanel.transform.GetChild(4).GetChild(1).GetComponent<Button>();
        ItemColor = TradePanel.transform.GetChild(5).GetComponent<Image>();
        ItemDamageText = TradePanel.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        ItemEffectText = TradePanel.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    //Dynamically adds the gameobject to the UI Panel button so you can select it
    protected void updateVendorTradeUI()
    {
        int ItemCost = -1;
        if (itemForSale.item == null)
        {
            ItemCost = -1;
            Debug.Log("You have purchased everything from this vendor.");
        }
        else if (itemForSale.item.GetComponent<RecipePickUp>())
        {
            ItemCost = itemForSale.item.GetComponent<RecipePickUp>().item.Cost;
            ItemNameText.text = itemForSale.item.name;
            ItemCostText.text = "Cost: " + ItemCost;
            ItemIcon.sprite = itemForSale.item.GetComponent<RecipePickUp>().item.icon;
            if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.PhysicalRecipe)
            {
                ItemColor.color = LootLedger.PhysicalColor;
            }
            else if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.GunPowderRecipe)
            {
                ItemColor.color = LootLedger.GunPowderColor;
            }
            else if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.ExplosiveRecipe)
            {
                ItemColor.color = LootLedger.ExplosiveColor;
            }
            ItemDamageText.text = itemForSale.item.GetComponent<RecipePickUp>().DamageDescription;
            ItemEffectText.text = itemForSale.item.GetComponent<RecipePickUp>().EffectDescription;
        }
        else if (itemForSale.item.GetComponent<PlayerGun>())
        {
            ItemCost = itemForSale.item.GetComponent<PlayerGun>().Cost;
            ItemNameText.text = itemForSale.item.name;
            ItemCostText.text = "Cost: " + ItemCost;
            ItemIcon.sprite = itemForSale.item.GetComponent<SpriteRenderer>().sprite;
            ItemColor.color = LootLedger.WeaponColor;
            ItemDamageText.text = itemForSale.item.GetComponent<PlayerGun>().DamageDescription;
            ItemEffectText.text = itemForSale.item.GetComponent<PlayerGun>().EffectDescription;
        }

        if (ItemCost >= 0 && ItemCost <= Inventory.Instance.getMoneyCount())
        {
            ItemBuyButton.enabled = true;
            ItemBuyButton.onClick.AddListener(() => { BuyItemButton(ItemCost); });
        }
        else
        {
            ItemBuyButton.enabled = false;
        }
        ItemExitButton.onClick.AddListener(() => { ExitButton(); });
    }

    private void BuyItemButton(int ItemCost)
    {
        TradePanel.SetActive(false);
        Player.Instance.enablePlayer(true);
        Time.timeScale = 1;

        displaySaveIcon();

        //Save Money
        for (int i = 0; i < ItemCost; i++)
        {
            Inventory.Instance.RemoveItem(Inventory.Instance.moneyCoin);
        }
        SaveSystem.SaveGlobalMoneyData(Inventory.Instance.getMoneyCount());

        //Save New Vendor Loot Table
        vendorInventory.Remove(itemForSale);
        SaveVendorLootPool SaveVendorLootPool = SaveSystem.LoadVendorLootPoolData();
        foreach (KeyValuePair<VendorType, List<string>> entry in SaveVendorLootPool.VendorLootPool)
        {
            if (entry.Key == vendorType)
            {
                entry.Value.Clear();
                foreach (Loot item in vendorInventory)
                {
                    entry.Value.Add(item.item.name);
                }
                break;
            }
        }
        SaveSystem.SaveVendorLootPoolData(SaveVendorLootPool.VendorLootPool);

        //Save New Player Loot Pool
        SavePlayerLootPool savePlayerLootPool = SaveSystem.LoadPlayerLootPoolData();
        if (itemForSale.item.GetComponent<RecipePickUp>())
        {
            if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.PhysicalRecipe)
            {
                if (!savePlayerLootPool.PlayerLootPoolDict[LootListType.PhysicalRecipe].Contains(itemForSale.item.name))
                {
                    savePlayerLootPool.PlayerLootPoolDict[LootListType.PhysicalRecipe].Add(itemForSale.item.name);
                }
            }
            if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.GunPowderRecipe)
            {
                if (!savePlayerLootPool.PlayerLootPoolDict[LootListType.GunpowderRecipe].Contains(itemForSale.item.name))
                {
                    savePlayerLootPool.PlayerLootPoolDict[LootListType.GunpowderRecipe].Add(itemForSale.item.name);
                }
            }
            if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == RecipeType.ExplosiveRecipe)
            {
                if (!savePlayerLootPool.PlayerLootPoolDict[LootListType.ExplosiveRecipe].Contains(itemForSale.item.name))
                {
                    savePlayerLootPool.PlayerLootPoolDict[LootListType.ExplosiveRecipe].Add(itemForSale.item.name);
                }
            }
        }
        if (itemForSale.item.GetComponent<PlayerGun>())
        {
            if (!savePlayerLootPool.PlayerLootPoolDict[LootListType.Weapon].Contains(itemForSale.item.name))
            {
                savePlayerLootPool.PlayerLootPoolDict[LootListType.Weapon].Add(itemForSale.item.name);
            }
        }
        SaveSystem.SavePlayerLootPoolData(savePlayerLootPool.PlayerLootPoolDict);
        RemoveAllListeners();
    }

    public void ExitButton()
    {
        TradePanel.SetActive(false);
        Player.Instance.enablePlayer(true);
        Time.timeScale = 1;
        RemoveAllListeners();
    }

    private void RemoveAllListeners()
    {
        ItemBuyButton.onClick.RemoveAllListeners();
        ItemExitButton.onClick.RemoveAllListeners();
    }

    private void displaySaveIcon()
    {
        GameObject saveIcon = GameObject.Find("Canvas").transform.Find("SaveIcon").gameObject;
        if (!saveIcon.activeInHierarchy)
        {
            saveIcon.SetActive(true);
        }
        StartCoroutine(LateCall(saveIcon));
    }

    private IEnumerator LateCall(GameObject saveIcon)
    {
        yield return new WaitForSeconds(2f);
        saveIcon.SetActive(false);
    }

    //Note: This is just a basic way to select the item. Later this method needs to be revised
    private void selectItemForSale()
    {
        itemForSale = vendorInventory[0];
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
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        //Vendor stops showing what Item he is carrying
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            weeklyItemPopup.SetActive(false);
            playerInRange = false;
        }
    }
}
public enum VendorType { Physical, Gunpowder, Explosive, Weapon }