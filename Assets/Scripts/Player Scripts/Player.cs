using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion
    public float speed = 10f;
    public Rigidbody2D myRigidBody;
    public PlayerAnimController animator;
    private bool movementEnabled = true;

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

    public void enablePlayer(Boolean playerUsable)
    {
        CameraController.Instance.enabled = playerUsable;
        CursorController.Instance.enabled = playerUsable;
        Transform currentWeapon = WeaponSwitching.Instance.getSelectedWeapon();
        currentWeapon.GetComponentInChildren<GunFiring>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunControls>().enabled = playerUsable;
        currentWeapon.GetComponentInChildren<GunProperties>().enabled = playerUsable;
        WeaponSwitching.Instance.enabled = playerUsable;
        animator.enabled = playerUsable;
        movementEnabled = playerUsable;
    }
}
