using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    public bool isInteractable = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

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
        Vector3 keyboardMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //running character follows keys
        animator.SetFloat("KeyBoardHorizontal", keyboardMove.x);
        animator.SetFloat("KeyBoardVertical", keyboardMove.y);
        animator.SetFloat("Magnitude", keyboardMove.magnitude);

        //Idle character follows camera
        animator.SetFloat("MouseHorizontal", mousePos.normalized.x);
        animator.SetFloat("MouseVertical", mousePos.normalized.y);

        //NOTE: ISSUE THAT IT WON'T TRANSITION BETWEEN STRAFE STATES UNLESS YOU STOP MOVING OR RUN FIRST
        //NOTE: WILL NEED TO CHANGE TO .25f VALUES FOR 8 DIRECTION
        //NOTE: POTENTIAL OPTION, USE ANIMATOR.PLAY(AND DELETE ALL TRANSITIONS??)

        //Look up strafe
        if((keyboardMove.x < 0 || keyboardMove.x > 0) && (mousePos.x >= -.5 || mousePos.x <= .5) && (mousePos.y > 0))
        {
            animator.SetInteger("strafePosition", 1);
            print("StrafingLookingUp");
        }

        //Look left strafe
        else if ((keyboardMove.y < 0 || keyboardMove.y > 0) && (mousePos.y >= -.5 || mousePos.y <= .5) && (mousePos.x < 0))
        {
            animator.SetInteger("strafePosition", 7);
            print("StrafingLookingLeft");
        }

        //Look right strafe
        else if ((keyboardMove.y < 0 || keyboardMove.y > 0) && (mousePos.y >= -.5 || mousePos.y <= .5) && (mousePos.x > 0))
        {
            animator.SetInteger("strafePosition", 3);
            print("StrafingLookingRight");
        }

        //Look down strafe
        else if ((keyboardMove.x < 0 || keyboardMove.x > 0) && (mousePos.x >= -.5 || mousePos.x <= .5) && (mousePos.y < 0))
        {
            animator.SetInteger("strafePosition", 5);
            print("StrafingLookingDown");
        }
        else
        {
            animator.SetInteger("strafePosition", 0);
        }

        //Move character
        transform.position += keyboardMove * speed * Time.deltaTime;
    }

    public void enablePlayer(Boolean playerUsable)
    {
        if (playerUsable)
        {
            this.enabled = true;
            GetComponent<CameraController>().enabled = true;
            GetComponent<CursorController>().enabled = true;
            GetComponentInChildren<GunFiring>().enabled = true;
            GetComponentInChildren<GunControls>().enabled = true;
            GetComponentInChildren<GunProperties>().enabled = false;
        }
        if (!playerUsable)
        {
            this.enabled = false;
            GetComponent<CameraController>().enabled = false;
            GetComponent<CursorController>().enabled = false;
            GetComponentInChildren<GunFiring>().enabled = false;
            GetComponentInChildren<GunControls>().enabled = false;
            GetComponentInChildren<GunProperties>().enabled = false;
        }
    }
}
