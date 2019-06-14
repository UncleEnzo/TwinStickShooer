using System;
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
    private EnemyManager enemyManager;

    void Start()
    {
        helpTextEnum = helpText();
        enemyManager = FindObjectOfType<EnemyManager>();
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
            Boolean lastSentence = FindObjectOfType<DialogueManager>().displayNextSentence();

            if (lastSentence)
            {
                enemyManager.instantiateRandomEnemies(6);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == FindObjectOfType<Player>().GetComponent<Collider2D>())
        {
            playerInField = true;
            StartCoroutine(helpTextEnum);
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D == FindObjectOfType<Player>().GetComponent<Collider2D>())
        {
            playerInField = false;
            animator.SetBool("isOpen", false);
            StopCoroutine(helpTextEnum);
        }
    }

    private IEnumerator helpText()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isOpen", true);
    }
}
