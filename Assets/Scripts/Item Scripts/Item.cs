using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public virtual void useItem()
    {
        //Use the item
        //Something may happen
        //Use this method to turn craftables into Pots/GunPowder/Explosives
        //Use this method to use potions/GunPowders/Explosives
        //(Explosives should maybe go straight to scroll wheel)
        Debug.Log("Using " + name);
    }
}
