using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void triggerDialogue()
    {
        Time.timeScale = 1f;
        Player.Instance.enablePlayer(false);
        InventoryUI.canUseUI = false;
        FindObjectOfType<DialogueManager>().startDialogue(dialogue);
    }
}
