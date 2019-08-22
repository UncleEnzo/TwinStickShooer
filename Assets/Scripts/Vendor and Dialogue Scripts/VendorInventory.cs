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
    private bool itemPurchased = false;

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
        if (itemPurchased == true)
        {
            print("Selecting new item");
            selectItemForSale(); // I think this is a janky solution because if you run out of items, this will constantly call null
            itemPurchased = false;
        }
        if (playerInRange && Input.GetKeyDown("e") && !InventoryUI.UIOpen)
        {
            Time.timeScale = 0;
            Player.Instance.enablePlayer(false);
            InventoryUI.canUseUI = false;
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
        if (itemForSale == null)
        {
            ItemNameText.text = "No Items Left To Sell";
            ItemCostText.text = "Cost: ";
            ItemIcon.sprite = null;
            ItemColor.color = new Color32(0, 0, 0, 0);
            ItemDamageText.text = "This Vendor is Itemless";
            ItemEffectText.text = "You bought all the vendor's items";
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
        InventoryUI.canUseUI = true;
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
        UpdatePlayerSavePool(savePlayerLootPool);
        SaveSystem.SavePlayerLootPoolData(savePlayerLootPool.PlayerLootPoolDict);
        itemPurchased = true;
        RemoveAllListeners();
    }

    private void UpdatePlayerSavePool(SavePlayerLootPool savePlayerLootPool)
    {
        if (itemForSale.item.GetComponent<RecipePickUp>())
        {
            DetemineRecipeType(RecipeType.PhysicalRecipe, LootListType.PhysicalRecipe, savePlayerLootPool);
            DetemineRecipeType(RecipeType.GunPowderRecipe, LootListType.GunpowderRecipe, savePlayerLootPool);
            DetemineRecipeType(RecipeType.ExplosiveRecipe, LootListType.ExplosiveRecipe, savePlayerLootPool);
        }
        else
        {
            addToList(LootListType.Weapon, savePlayerLootPool);
        }
    }

    private void DetemineRecipeType(RecipeType recipe, LootListType lootList, SavePlayerLootPool savePlayerLootPool)
    {
        if (itemForSale.item.GetComponent<RecipePickUp>().recipeType == recipe)
        {
            addToList(lootList, savePlayerLootPool);
        }
    }

    private void addToList(LootListType lootListType, SavePlayerLootPool savePlayerLootPool)
    {
        if (!savePlayerLootPool.PlayerLootPoolDict[lootListType].Contains(itemForSale.item.name))
        {
            savePlayerLootPool.PlayerLootPoolDict[lootListType].Add(itemForSale.item.name);
        }
    }

    public void ExitButton()
    {
        TradePanel.SetActive(false);
        Player.Instance.enablePlayer(true);
        InventoryUI.canUseUI = true;
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

    private void selectItemForSale()
    {
        itemForSale = LootTable.instance.generateRandomLoot(vendorInventory);
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        //Vendor raises signal showing what Item he is carrying
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            if (itemForSale != null)
            {
                if (itemForSale.item.GetComponent<RecipePickUp>())
                {
                    weeklyItemPopup.GetComponentsInChildren<SpriteRenderer>()[1].sprite = itemForSale.item.GetComponent<RecipePickUp>().item.icon;
                    weeklyItemPopup.SetActive(true);
                }
                else
                {
                    weeklyItemPopup.GetComponentsInChildren<SpriteRenderer>()[1].sprite = itemForSale.item.GetComponent<SpriteRenderer>().sprite;
                    weeklyItemPopup.SetActive(true);
                }
            }
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