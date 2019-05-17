using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator topAnimator;
    public Animator bottomAnimator;

    public void Update()
    {
        Vector3 keyboardMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //Idle character body follows camera
        //animator.SetFloat("MouseHorizontal", mousePos.normalized.x);
        //animator.SetFloat("MouseVertical", mousePos.normalized.y);

        //running character legs follow keys
        bottomAnimator.SetFloat("KeyBoardHorizontal", keyboardMove.x);
        bottomAnimator.SetFloat("KeyBoardVertical", keyboardMove.y);
        bottomAnimator.SetFloat("KeyBoardMagnitude", keyboardMove.magnitude);

        //running character torso follows mouse cursor
        topAnimator.SetFloat("MouseHorizontal", mousePos.normalized.x);
        topAnimator.SetFloat("MouseVertical", mousePos.normalized.y);
        topAnimator.SetFloat("KeyBoardMagnitude", keyboardMove.magnitude);
    }
}
