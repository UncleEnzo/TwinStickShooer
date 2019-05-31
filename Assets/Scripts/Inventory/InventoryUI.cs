using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    public Player player;
    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallBack += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            if (inventoryUI.activeSelf == true)
            {
                player.enablePlayer(false);
                Time.timeScale = .2f;
            }
            if (inventoryUI.activeSelf == false)
            {
                player.enablePlayer(true);
                Time.timeScale = 1f;
            }
        }
    }

    //Need to update this so that if you pick up the same item you have multiple
    //need to add a counter somehow
    //need to add the ability for the items to be spent
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
