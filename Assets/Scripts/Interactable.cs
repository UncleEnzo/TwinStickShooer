using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Interactable interactable;

    public virtual void interact()
    {
        print("Interaction or PickUp available");
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && gameObject.tag == "Interactable")
        {
            collider2D.GetComponent<Player>().isInteractable = true;
            interact();
        }

        if (collider2D.tag == "Player" && gameObject.tag == "PickUp")
        {
            interact();
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            collider2D.GetComponent<Player>().isInteractable = false;
        }
    }
}
