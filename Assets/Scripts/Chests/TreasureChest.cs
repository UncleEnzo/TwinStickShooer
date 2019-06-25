using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public GameObject contents;
    public bool isOpen;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Need to make sure the player has a key?? SHOULD CHESTS TAKE KEYS????
        if (Input.GetKeyDown("e") && playerInRange)
        {
            if (!isOpen)
            {
                //open the chest
                OpenChest();
            }
            else
            {
                //Chest is already Open
                ChestIsAlreadyOpen();
            }
        }
    }
    public void OpenChest()
    {
        isOpen = true;
        //Drop Items >> Instantiate something from the pool of items based on chest rarity
        anim.SetBool("opened", true);
        Instantiate(contents, new Vector2(transform.position.x, transform.position.y - 3f), Quaternion.identity);
        //Recieve a signal that one was picked and send out another one for the other to be turned off
    }
    public void ChestIsAlreadyOpen()
    {
        Debug.Log("Chest is already Open");
    }
}
