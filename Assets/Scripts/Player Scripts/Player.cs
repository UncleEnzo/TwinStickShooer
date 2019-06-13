using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public bool isInteractable = false;
    public Rigidbody2D keyboardMoveRigidBody2D;
    Vector3 movement;

    void Update()
    {
        if (Input.GetKeyDown("e") && isInteractable == true)
        {
            print("Interacting with NPC or object");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        // movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        // if (movement.magnitude > 1.0f)
        // {
        //     movement.Normalize();
        // }
        // keyboardMoveRigidBody2D.velocity = new Vector2(movement.x, movement.y);// * speed
        Vector3 tempVect = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        keyboardMoveRigidBody2D.MovePosition(transform.position + tempVect);
    }

    public void enablePlayer(Boolean playerUsable)
    {
        GameObject weaponHolder = GameObject.FindWithTag("WeaponHolder");
        CameraController camControl = FindObjectOfType<CameraController>();

        camControl.enabled = playerUsable;
        GetComponent<CursorController>().enabled = playerUsable;
        Transform currentWeapon = weaponHolder.GetComponent<WeaponSwitching>().getSelectedWeapon();
        currentWeapon.GetComponentInChildren<GunFiring>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunControls>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunProperties>().enabled = playerUsable;
        weaponHolder.GetComponent<WeaponSwitching>().enabled = playerUsable;
        GetComponentInChildren<PlayerAnimController>().isEnabled = playerUsable;
        this.enabled = playerUsable;
    }
}
