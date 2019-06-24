using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool playerInRange = false;
    public virtual void interact()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && collider2D.isTrigger && gameObject.tag == "Interactable")
        {
            playerInRange = true;
            print(playerInRange);

            interact();
        }

        if (collider2D.tag == "Player" && collider2D.isTrigger && gameObject.tag == "PickUp")
        {
            interact();
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && collider2D.isTrigger)
        {
            playerInRange = false;
            print(playerInRange);
        }
    }
}