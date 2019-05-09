﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : MonoBehaviour
{
    private Boolean playerInField;
    public Boolean dialogueTriggered = false;
    private float nextDialogue = 1F;
    public float speechRate = 1F;
    public Animator animator;
    private IEnumerator helpTextEnum;

    void Start()
    {
        helpTextEnum = helpText();
    }
    void Update()
    {
        if (playerInField && !dialogueTriggered && Input.GetKeyDown("e") && Time.time > nextDialogue)
        {
            StopCoroutine(helpTextEnum);
            animator.SetBool("isOpen", false);
            nextDialogue = Time.time + speechRate;
            dialogueTriggered = true;
            GetComponent<DialogueTrigger>().triggerDialogue();
        }


        if ((dialogueTriggered && Input.GetKeyDown("e") && Time.time > nextDialogue) || (dialogueTriggered && Input.GetMouseButtonDown(0) && Time.time > nextDialogue))
        {
            nextDialogue = Time.time + speechRate;
            FindObjectOfType<DialogueManager>().displayNextSentence();
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D == FindObjectOfType<Player>().GetComponent<CircleCollider2D>())
        {
            playerInField = true;
            StartCoroutine(helpTextEnum);
        }
    }

    private IEnumerator helpText()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isOpen", true);
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D == FindObjectOfType<Player>().GetComponent<CircleCollider2D>())
        {
            playerInField = false;
            animator.SetBool("isOpen", false);
            StopCoroutine(helpTextEnum);
        }
    }
}
