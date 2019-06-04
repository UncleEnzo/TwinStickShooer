using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Player player;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        openOrCloseInventory();
    }

    private void openOrCloseInventory()
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
}