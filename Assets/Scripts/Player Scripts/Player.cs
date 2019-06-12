using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
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
