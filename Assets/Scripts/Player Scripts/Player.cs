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
    public bool movementEnabled = true;
    private float coolDownOnMovement = 1f;
    public bool playerUsable = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        //CoolDown for parry
        if (!movementEnabled)
        {
            coolDownOnMovement -= Time.deltaTime;
            print("Cooling down");
            if (coolDownOnMovement <= 0)
            {
                movementEnabled = true;
            }
        }
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
        this.playerUsable = playerUsable;
        CameraController.Instance.enabled = playerUsable;
        CursorController.Instance.enabled = playerUsable;
        WeaponSwitching.Instance.GetComponent<SetGunPosition>().enabled = playerUsable;
        Transform currentWeapon = WeaponSwitching.Instance.getSelectedWeapon();
        currentWeapon.GetComponentInChildren<Weapon>().enabled = playerUsable;
        animator.enabled = playerUsable;
        movementEnabled = playerUsable;
    }
}
