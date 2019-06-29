using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody2D myRigidBody;
    public static PlayerAnimController animator;
    Vector3 movement;
    private static bool movementEnabled = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementEnabled)
        {
            Move();
        }

    }

    public void Move()
    {
        Vector3 tempVect = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        myRigidBody.MovePosition(transform.position + tempVect);
    }

    public static void enablePlayer(Boolean playerUsable)
    {
        CameraController.Instance.enabled = playerUsable;
        CursorController.Instance.enabled = playerUsable;
        Transform currentWeapon = WeaponSwitching.Instance.getSelectedWeapon();
        currentWeapon.GetComponentInChildren<GunFiring>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunControls>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunProperties>().enabled = playerUsable;
        WeaponSwitching.Instance.enabled = playerUsable;
        animator.isEnabled = playerUsable;
        movementEnabled = playerUsable;
    }
}
