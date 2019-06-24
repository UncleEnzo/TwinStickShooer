using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator topAnimator;
    public Animator bottomAnimator;
    public bool isEnabled = true;
    public void Update()
    {
        Vector3 keyboardMove = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float mousePosX = mousePos.normalized.x;
        float mousePosY = mousePos.normalized.y;

        //Idle character body follows camera
        topAnimator.SetFloat("IdleMousePosHorizontal", mousePosX);
        topAnimator.SetFloat("IdleMousePosVertical", mousePosY);
        bottomAnimator.SetFloat("IdleMousePosHorizontal", mousePosX);
        bottomAnimator.SetFloat("IdleMousePosVertical", mousePosY);
        if (isEnabled)
        {
            animationInputs(mousePosX, mousePosY, keyboardMove.x, keyboardMove.y, keyboardMove.magnitude);
        }
        else
        {
            animationInputs(mousePosX, mousePosY, 0f, 0f, 0f);
        }
    }

    private void animationInputs(float mousePosX, float mousePosY, float keyboardMoveX, float keyboardMoveY, float keyboardMoveMag)
    {
        //running character torso follows mouse cursor
        topAnimator.SetFloat("MouseHorizontal", mousePosX);
        topAnimator.SetFloat("MouseVertical", mousePosY);
        topAnimator.SetFloat("KeyBoardMagnitude", keyboardMoveMag);

        //running character legs follow keys
        bottomAnimator.SetFloat("KeyBoardHorizontal", keyboardMoveX);
        bottomAnimator.SetFloat("KeyBoardVertical", keyboardMoveY);
        bottomAnimator.SetFloat("KeyBoardMagnitude", keyboardMoveMag);

    }
}
