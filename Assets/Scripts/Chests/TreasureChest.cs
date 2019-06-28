using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public bool isOpen;
    public Item key;
    public int chestRarityRange;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected virtual int OpenChest()
    {
        isOpen = true;
        //Drop Items >> Add Rarity
        anim.SetBool("opened", true);
        return Random.Range(0, 100000);
    }

    protected bool attemptToOpenChest()
    {
        bool success = false;
        if (Input.GetKeyDown("e") && playerInRange)
        {
            if (Inventory.instance.getKeyCount() > 0)
            {
                if (!isOpen)
                {
                    Collider2D[] result = Physics2D.OverlapCircleAll(transform.position, 4f, 3);
                    if (result.Count() == 2)
                    {
                        //open the chest
                        Inventory.instance.RemoveItem(key);
                        success = true;
                        return success;
                    }
                    else
                    {
                        print("SOMETHING IS TOO CLOSE TO THE CHEST FOR IT TO OPEN");
                        print("Result count = " + result.Count());
                        return success;
                    }
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

    protected void spawnRecipe(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject recipe = Instantiate(LootTable.instance.generateRandomLoot(loot, chestRarityRange), new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
        recipe.GetComponent<Recipe>().chestID = chestID;
        recipe.GetComponent<Recipe>().isFromChest = true;
    }

    protected void spawnItem(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject item = Instantiate(LootTable.instance.generateRandomLoot(loot, chestRarityRange), new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
    }
}
