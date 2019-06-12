using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int inventoryId = 0;
    public Image icon;
    public Text txtCount;
    private Stack<Item> itemStack = new Stack<Item>();
    public void Start()
    {
        txtCount.text = "";
    }
    public int Id
    {
        get { return inventoryId; }
    }
    public void AddItem(Item item)
    {
        item.inventorySlot = this;
        itemStack.Push(item);
    }
    public Item firstItem
    {
        get
        {
            if (isEmpty)
            {
                return null;
            }
            else
            {
                return itemStack.Peek();
            }
        }
    }
    public bool isStackable(Item item)
    {
        if (isEmpty)
        {
            return false;
        }
        Item first = itemStack.Peek();
        if (first.name == item.name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isEmpty
    {
        get
        {
            return Count == 0;
        }
    }
    public bool Remove(Item item)
    {
        if (isEmpty)
        {
            return false;
        }
        Item first = itemStack.Peek();
        if (first.name == item.name)
        {
            itemStack.Pop();
            return true;
        }
        else
        {
            return false;
        }
    }
    public int Count
    {
        get
        {
            return itemStack.Count;
        }
    }

    //Called when button is pressed, then goes to that item and calls its useItem Function()
    public void useItem()
    {
        if (firstItem != null)
        {
            firstItem.useItem();
        }
    }
}
