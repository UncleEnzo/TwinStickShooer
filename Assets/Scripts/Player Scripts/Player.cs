using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public bool isInteractable = false;

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown("e") && isInteractable == true)
        {
            print("Interacting with NPC or object");
        }
    }

    public void Move()
    {
        //Move character
        Vector3 keyboardMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += keyboardMove * speed * Time.deltaTime;
    }

    public void enablePlayer(Boolean playerUsable)
    {
        if (playerUsable)
        {
            this.enabled = true;
            FindObjectOfType<CameraController>().enabled = true;
            GetComponent<CursorController>().enabled = true;
            GetComponentInChildren<GunFiring>().enabled = true;
            GetComponentInChildren<GunControls>().enabled = true;
            GetComponentInChildren<GunProperties>().enabled = false;
        }
        if (!playerUsable)
        {
            this.enabled = false;
            FindObjectOfType<CameraController>().enabled = false;
            GetComponent<CursorController>().enabled = false;
            GetComponentInChildren<GunFiring>().enabled = false;
            GetComponentInChildren<GunControls>().enabled = false;
            GetComponentInChildren<GunProperties>().enabled = false;
        }
    }
}
