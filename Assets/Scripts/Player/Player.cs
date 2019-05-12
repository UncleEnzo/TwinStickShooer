using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public bool isInteractable = false;
    public bool isItemPickUp = false;

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown("e") && isInteractable == true)
        {
            print("WE GOT IN BABY");
        }

        if(isItemPickUp == true)
        {
            print("WE PICKED IT THE FUCK UP");
            print(isItemPickUp);
        }
    }

    public void Move()
    {
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += Movement * speed * Time.deltaTime;
    }

    public void enablePlayer(Boolean playerUsable)
    {
        if (playerUsable)
        {
            FindObjectOfType<Player>().enabled = true;
            FindObjectOfType<CursorController>().enabled = true;
            FindObjectOfType<CameraController>().enabled = true;
            FindObjectOfType<GunFiring>().enabled = true;
        }
        if (!playerUsable)
        {
            FindObjectOfType<Player>().enabled = false;
            FindObjectOfType<CursorController>().enabled = false;
            FindObjectOfType<CameraController>().enabled = false;
            FindObjectOfType<GunFiring>().enabled = false;
        }
    }
}
