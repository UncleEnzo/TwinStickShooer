﻿using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Interactable
{
    public Item contents;
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
        //drop items
        //Recieve a signal that one was picked and send out another one for the other to be turned off
        isOpen = true;

    }
    public void ChestIsAlreadyOpen()
    {

    }
}
