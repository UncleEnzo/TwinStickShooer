using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : Interactable
{
    public Signal ToolTipOn;
    public Signal ToolTipOff;
    public Boolean dialogueTriggered = false;
    private float nextDialogue = 1F;
    public float speechRate = 1F;

    void Update()
    {
        if (playerInRange && !dialogueTriggered && Input.GetKeyDown("e") && !InventoryUI.UIOpen && Time.time > nextDialogue)
        {
            print("Dialogue Triggered");
            ToolTipOff.Raise();
            nextDialogue = Time.time + speechRate;
            dialogueTriggered = true;
            GetComponent<DialogueTrigger>().triggerDialogue();
        }


        if ((dialogueTriggered && Input.GetKeyDown("e") && !InventoryUI.UIOpen && Time.time > nextDialogue) || (dialogueTriggered && Input.GetMouseButtonDown(0) && Time.time > nextDialogue))
        {
            nextDialogue = Time.time + speechRate;
            Boolean lastSentence = FindObjectOfType<DialogueManager>().displayNextSentence();

            if (lastSentence)
            {
                EnemySpawner.Instance.activateRandomEnemies(6);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            playerInRange = true;
            ToolTipOn.Raise();
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D == Player.Instance.GetComponent<Collider2D>())
        {
            playerInRange = false;
            ToolTipOff.Raise();
        }
    }
}
