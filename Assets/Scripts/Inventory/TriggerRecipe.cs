using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRecipe : MonoBehaviour
{
    public Item item;
    public void useRecipe()
    {
        if (item != null)
        {
            item.useItem();
        }
    }
}
