using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static bool canUseUI = true;
    public static float UITimeScale = .1f;
    public static bool UIOpen;

    void Update()
    {
        if (Input.GetButtonDown("Inventory")
         && canUseUI
         && (Player.Instance.playerState == PlayerStates.MovingShooting
         || Player.Instance.playerState == PlayerStates.Disabled))
        {
            openOrCloseInventory();
        }
    }

    public static void openOrCloseInventory()
    {
        UIOpen = !UIOpen;
        if (UIOpen == true)
        {
            Player.Instance.enablePlayer(false);
            Time.timeScale = UITimeScale;
            foreach (GameObject recipeIcon in Inventory.Instance.recipeIcons)
            {
                recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = true;
            }
        }
        if (UIOpen == false)
        {
            Player.Instance.enablePlayer(true);
            Time.timeScale = 1f;
            foreach (GameObject recipeIcon in Inventory.Instance.recipeIcons)
            {
                recipeIcon.transform.GetChild(1).GetComponent<Button>().interactable = false;
            }
        }
    }
}