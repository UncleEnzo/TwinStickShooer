using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Player player;
    Inventory inventory;

    private bool UIOpen;

    // Start is called before the first frame update
    void Start()
    {
        //Ensures that the UI is enabled, then deactived on start up, making it usable right away
        //If it starts deactivated, it will not update until it is opened once
        player = FindObjectOfType<Player>();
        inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        openOrCloseInventory();
    }

    //Needs rework, The actual inventory ui should always be open.
    //What whould change is that when you press tab, time slows and recipies become clickable
    private void openOrCloseInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            UIOpen = !UIOpen;
            if (UIOpen == true)
            {
                player.enablePlayer(false);
                Time.timeScale = .2f;
                foreach (GameObject recipeIcon in inventory.recipeIcons)
                {
                    recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
            }
            if (UIOpen == false)
            {
                player.enablePlayer(true);
                Time.timeScale = 1f;
                foreach (GameObject recipeIcon in inventory.recipeIcons)
                {
                    recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = false;
                }
            }
        }
    }
}