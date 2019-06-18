using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : MonoBehaviour
{
    public Signal ToolTipOn;
    public Signal ToolTipOff;
    private Boolean playerInField;
    public Boolean dialogueTriggered = false;
    private float nextDialogue = 1F;
    public float speechRate = 1F;
    private EnemySpawner enemySpawner;

    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    void Update()
    {
        if (playerInField && !dialogueTriggered && Input.GetKeyDown("e") && Time.time > nextDialogue)
        {
            ToolTipOff.Raise();
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
                enemySpawner.instantiateRandomEnemies(6);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == FindObjectOfType<Player>().GetComponent<Collider2D>())
        {
            playerInField = true;
            ToolTipOn.Raise();
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D == FindObjectOfType<Player>().GetComponent<Collider2D>())
        {
            ToolTipOff.Raise();
            playerInField = false;
        }
    }
}
