using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int speed = 10;


    // Update is called once per frame
    void Update()
    {
        Move();
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
            FindObjectOfType<Gun>().enabled = true;
        }
        if (!playerUsable)
        {
            FindObjectOfType<Player>().enabled = false;
            FindObjectOfType<CursorController>().enabled = false;
            FindObjectOfType<CameraController>().enabled = false;
            FindObjectOfType<Gun>().enabled = false;
        }
    }
}
