using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public Animator animator;


    public void Enable()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }
        else
        {
            Debug.Log("ToolTip animator not set to an instance of an object!");
        }
    }

    public void Disable()
    {
        if (animator != null)
        {
            animator.SetBool("isOpen", false);
        }
        else
        {
            Debug.Log("ToolTip animator not set to an instance of an object!");
        }
    }
}
