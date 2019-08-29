using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : Interactable
{
    public Signal recipePicked;
    public float health;
    public bool isOpen;
    public Item key;
    [Header("Lower rarity range means rarer items become more ")]
    public int chestRarityRange;
    private Animator anim;
    protected GameObject RecipeUIPanel;

    [Header("List of Recipe UI Objects that are updated when you open a chest")]
    protected GameObject physicalPanel;
    protected Image physicalIcon;
    protected Image physicalDamageTextColor;
    protected TextMeshProUGUI damageTextPhysical;
    protected TextMeshProUGUI physicalEffectText;
    protected TextMeshProUGUI physicalCraftPhysText;
    protected TextMeshProUGUI physicalCraftGunpowderText;
    protected TextMeshProUGUI physicalCraftExplosiveText;
    protected Button physicalButton;
    protected GameObject gunpowderPanel;
    protected Image gunpowderIcon;
    protected Image gunpowderDamageTextColor;
    protected TextMeshProUGUI damageTextGunPowder;
    protected TextMeshProUGUI gunPowderEffectText;
    protected TextMeshProUGUI gunpowderCraftPhysText;
    protected TextMeshProUGUI gunpowderCraftGunpowderText;
    protected TextMeshProUGUI gunpowderCraftExplosiveText;
    protected Button gunPowderButton;
    protected GameObject explosivePanel;
    protected Image explosiveIcon;
    protected Image explosiveDamageTextColor;
    protected TextMeshProUGUI damageTextExplosive;
    protected TextMeshProUGUI explosiveEffectText;
    protected TextMeshProUGUI explosiveCraftPhysText;
    protected TextMeshProUGUI explosiveCraftGunpowderText;
    protected TextMeshProUGUI explosiveCraftExplosiveText;
    protected Button explosiveButton;

    protected int chestID;
    void Awake()
    {
        anim = GetComponent<Animator>();
        RecipeUIPanel = GameObject.Find("Canvas").transform.Find("RecipeSelectMenu").gameObject;
    }

    protected virtual int OpenChest()
    {
        isOpen = true;
        anim.SetBool("opened", true);
        chestID = Random.Range(0, 100000);
        return chestID;
    }

    protected void GetRecipePanelUIButtons()
    {
        physicalPanel = RecipeUIPanel.transform.GetChild(1).gameObject;
        physicalIcon = physicalPanel.transform.GetChild(0).GetComponent<Image>();
        physicalDamageTextColor = physicalPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        damageTextPhysical = physicalPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalEffectText = physicalPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalCraftPhysText = physicalPanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        physicalCraftGunpowderText = physicalPanel.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        physicalCraftExplosiveText = physicalPanel.transform.GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        physicalButton = physicalPanel.transform.GetChild(3).GetComponent<Button>();
        gunpowderPanel = RecipeUIPanel.transform.GetChild(2).gameObject;
        gunpowderIcon = gunpowderPanel.transform.GetChild(0).GetComponent<Image>();
        gunpowderDamageTextColor = gunpowderPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        damageTextGunPowder = gunpowderPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderEffectText = gunpowderPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunpowderCraftPhysText = gunpowderPanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        gunpowderCraftGunpowderText = gunpowderPanel.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        gunpowderCraftExplosiveText = gunpowderPanel.transform.GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        gunPowderButton = gunpowderPanel.transform.GetChild(3).GetComponent<Button>();
        explosivePanel = RecipeUIPanel.transform.GetChild(3).gameObject;
        explosiveIcon = explosivePanel.transform.GetChild(0).GetComponent<Image>();
        explosiveDamageTextColor = explosivePanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        damageTextExplosive = explosivePanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveEffectText = explosivePanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveCraftPhysText = explosivePanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        explosiveCraftGunpowderText = explosivePanel.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        explosiveCraftExplosiveText = explosivePanel.transform.GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        explosiveButton = explosivePanel.transform.GetChild(3).GetComponent<Button>();
    }

    //Dynamically adds the gameobject to the UI Panel button so you can select it
    protected void updateRecipeUI(GameObject recipe, Image UISprite, TextMeshProUGUI UIDamage,
     TextMeshProUGUI UIEffect, TextMeshProUGUI craftPhysText, TextMeshProUGUI craftGunpowderText,
     TextMeshProUGUI craftExplosiveText, Button button)
    {
        if (recipe == null)
        {
            UISprite.sprite = null;
            UIDamage.text = "Place Holder";
            UIEffect.text = "Increase Player Health.";
            craftPhysText.text = "";
            craftGunpowderText.text = "";
            craftExplosiveText.text = "";
            button.onClick.AddListener(() => { NullRecipeSelection(); });
        }
        else
        {
            UISprite.sprite = recipe.GetComponent<RecipePickUp>().item.icon;
            UIDamage.text = recipe.GetComponent<RecipePickUp>().DamageDescription;
            UIEffect.text = recipe.GetComponent<RecipePickUp>().EffectDescription;
            craftPhysText.text = recipe.GetComponent<RecipePickUp>().recipe.physicalRequirement.ToString();
            craftGunpowderText.text = recipe.GetComponent<RecipePickUp>().recipe.gunPowderRequirement.ToString();
            craftExplosiveText.text = recipe.GetComponent<RecipePickUp>().recipe.explosiveRequirement.ToString();
            button.onClick.AddListener(() => { recipe.GetComponent<RecipePickUp>().MakeSelection(); });
        }
    }

    public void NullRecipeSelection()
    {
        GameObject RecipeUIPanel = GameObject.Find("Canvas").transform.Find("RecipeSelectMenu").gameObject;
        RecipeUIPanel.SetActive(false);
        recipePicked.Raise();
        Player.Instance.enablePlayer(true);
        InventoryUI.canUseUI = true;
        PauseMenu.otherMenuOpen = false;
        Player.Instance.totalHealth++;
        Player.Instance.health++;
        PlayerHUBController.Instance.updateDisplayHubHealth(Player.Instance.health, Player.Instance.totalHealth);
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

    protected void CheckDestroyChestHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected bool attemptToOpenChest()
    {
        bool success = false;
        if (Input.GetKeyDown("e") && playerInRange && !InventoryUI.UIOpen)
        {
            if (Inventory.Instance.getKeyCount() > 0)
            {
                if (!isOpen)
                {
                    //open the chest
                    Inventory.Instance.RemoveItem(key);
                    success = true;
                    return success;
                }
                else
                {
                    //Chest is already Open
                    ChestIsAlreadyOpen();
                    return success;
                }
            }
            return success;
        }
        return success;
    }

    private void ChestIsAlreadyOpen()
    {
        Debug.Log("Not Enough Keys or Chest is already Open");
    }

    protected GameObject spawnRecipe(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        Loot generatedLoot = null;
        generatedLoot = LootTable.instance.generateRandomLootFromDeductable(loot, chestRarityRange);
        if (generatedLoot != null)
        {
            GameObject recipe = Instantiate(generatedLoot.item, new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
            recipe.GetComponent<RecipePickUp>().chestID = chestID;
            recipe.GetComponent<RecipePickUp>().isFromChest = true;
            return recipe;
        }
        else
        {
            return null;
        }
    }

    protected void spawnItem(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject item = Instantiate(LootTable.instance.generateRandomLootFromDeductable(loot, chestRarityRange).item, new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
    }
}
