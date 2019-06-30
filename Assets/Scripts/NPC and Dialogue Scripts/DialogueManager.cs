using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private GameObject dialogueBox;
    private Text nameText;
    private Text dialogueText;
    private Animator animator;
    public TestNPC testNPC;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        nameText = dialogueBox.transform.GetChild(0).GetComponent<Text>();
        dialogueText = dialogueBox.transform.GetChild(1).GetComponent<Text>();
        animator = dialogueBox.GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    public void startDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        displayNextSentence();
    }
    public Boolean displayNextSentence()
    {
        Boolean lastSentence = false;
        if (sentences.Count == 0)
        {
            endDialogue();
            return lastSentence = true;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        return lastSentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void endDialogue()
    {
        animator.SetBool("isOpen", false);
        testNPC.dialogueTriggered = false;
        Player.Instance.enablePlayer(true);
    }
}
