using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public float health;
    public bool isOpen;
    public Item key;
    public int chestRarityRange;
    private Animator anim;
    protected GameObject RecipeUIPanel;
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
        GameObject recipe = Instantiate(LootTable.instance.generateRandomLoot(loot, chestRarityRange), new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
        recipe.GetComponent<RecipePickUp>().chestID = chestID;
        recipe.GetComponent<RecipePickUp>().isFromChest = true;
        return recipe;
    }

    protected void spawnItem(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject item = Instantiate(LootTable.instance.generateRandomLoot(loot, chestRarityRange), new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
    }
}
