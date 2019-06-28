using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public bool isOpen;
    public Item key;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
                        OpenChest();
                    }
                    else
                    {
                        print("SOMETHING IS TOO CLOSE TO THE CHEST FOR IT TO OPEN");
                        print("Result count = " + result.Count());
                    }
                }
                else
                {
                    //Chest is already Open
                    ChestIsAlreadyOpen();
                }
            }
        }
    }
    public void OpenChest()
    {
        isOpen = true;
        //Drop Items >> Instantiate something from the pool of items based on chest rarity
        anim.SetBool("opened", true);
        int chestID = Random.Range(0, 100000);
        spawnRecipe(LootListType.PhysicalRecipe, -3, -3, chestID);
        spawnRecipe(LootListType.GunpowderRecipe, 0, 3, chestID);
        spawnRecipe(LootListType.ExplosiveRecipe, 3, -3, chestID);
    }
    public void ChestIsAlreadyOpen()
    {
        Debug.Log("Not Enough Keys or Chest is already Open");
    }

    private void spawnRecipe(LootListType loot, float coordinateX, float coordinateY, int chestID)
    {
        GameObject recipe = Instantiate(LootTable.instance.generateRandomLoot(loot), new Vector2(transform.position.x + coordinateX, transform.position.y + coordinateY), Quaternion.identity);
        recipe.GetComponent<Recipe>().chestID = chestID;
        recipe.GetComponent<Recipe>().isFromChest = true;
    }
}
