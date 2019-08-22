using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool playerInRange = false;
    public virtual void interact()
    {
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == TagsAndLabels.PlayerTag && collider2D.isTrigger && gameObject.tag == TagsAndLabels.InteractableTag)
        {
            playerInRange = true;
            interact();
        }

        if (collider2D.tag == TagsAndLabels.PlayerTag && collider2D.isTrigger && gameObject.tag == TagsAndLabels.PickUpTag)
        {
            interact();
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == TagsAndLabels.PlayerTag && collider2D.isTrigger)
        {
            playerInRange = false;
            interact();
        }
    }
}