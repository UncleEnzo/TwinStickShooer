using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private bool UIOpen;

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
                Player.enablePlayer(false);
                Time.timeScale = .2f;
                foreach (GameObject recipeIcon in Inventory.Instance.recipeIcons)
                {
                    recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = true;
                }
            }
            if (UIOpen == false)
            {
                Player.enablePlayer(true);
                Time.timeScale = 1f;
                foreach (GameObject recipeIcon in Inventory.Instance.recipeIcons)
                {
                    recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = false;
                }
            }
        }
    }
}