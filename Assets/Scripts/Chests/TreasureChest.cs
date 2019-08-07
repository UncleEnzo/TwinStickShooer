using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : Interactable
{
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
    protected TextMeshProUGUI damageTextPhysical;
    protected TextMeshProUGUI physicalEffectText;
    protected Button physicalButton;
    protected GameObject gunpowderPanel;
    protected Image gunpowderIcon;
    protected TextMeshProUGUI damageTextGunPowder;
    protected TextMeshProUGUI gunPowderEffectText;
    protected Button gunPowderButton;
    protected GameObject explosivePanel;
    protected Image explosiveIcon;
    protected TextMeshProUGUI damageTextExplosive;
    protected TextMeshProUGUI explosiveEffectText;
    protected Button explosiveButton;
    void Awake()
    {
        anim = GetComponent<Animator>();
        RecipeUIPanel = GameObject.Find("Canvas").transform.Find("RecipeSelectMenu").gameObject;
    }

    protected virtual int OpenChest()
    {
        isOpen = true;
        anim.SetBool("opened", true);
        return Random.Range(0, 100000);
    }

    protected void GetRecipePanelUIButtons()
    {
        physicalPanel = RecipeUIPanel.transform.GetChild(1).gameObject;
        physicalIcon = physicalPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextPhysical = physicalPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalEffectText = physicalPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalButton = physicalPanel.transform.GetChild(3).GetComponent<Button>();
        gunpowderPanel = RecipeUIPanel.transform.GetChild(2).gameObject;
        gunpowderIcon = gunpowderPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextGunPowder = gunpowderPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderEffectText = gunpowderPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderButton = gunpowderPanel.transform.GetChild(3).GetComponent<Button>();
        explosivePanel = RecipeUIPanel.transform.GetChild(3).gameObject;
        explosiveIcon = explosivePanel.transform.GetChild(0).GetComponent<Image>();
        damageTextExplosive = explosivePanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveEffectText = explosivePanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveButton = explosivePanel.transform.GetChild(3).GetComponent<Button>();
    }

    //Dynamically adds the gameobject to the UI Panel button so you can select it
    protected void updateRecipeUI(GameObject recipe, Image UISprite, TextMeshProUGUI UIDamage, TextMeshProUGUI UIEffect, Button button)
    {
        if (recipe == null)
        {
            UISprite.sprite = null;
            UIDamage.text = "Place Holder";
            UIEffect.text = "Place Holder. If null, should offer player health and gundamage for reaching end of list";
        }
        else
        {
            UISprite.sprite = recipe.GetComponent<RecipePickUp>().item.icon;
            UIDamage.text = recipe.GetComponent<RecipePickUp>().DamageDescription;
            UIEffect.text = recipe.GetComponent<RecipePickUp>().EffectDescription;
            button.onClick.AddListener(() => { recipe.GetComponent<RecipePickUp>().MakeSelection(); });
        }
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
        if (Input.GetKeyDown("e") && playerInRange)
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
        GameObject item = LootTable.instance.generateRandomLootFromDeductable(loot, chestRarityRange).item;
        if (item != null)
        {
            GameObject recipe = Instantiate(LootTable.instance.generateRandomLootFromDeductable(loot, chestRarityRange).item, new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
            recipe.GetComponent<RecipePickUp>().chestID = chestID;
            recipe.GetComponent<RecipePickUp>().isFromChest = true;
            return recipe;
        }
        else
        {
            return item;
        }

    }

    protected void spawnItem(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject item = Instantiate(LootTable.instance.generateRandomLootFromDeductable(loot, chestRarityRange).item, new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
    }
}
