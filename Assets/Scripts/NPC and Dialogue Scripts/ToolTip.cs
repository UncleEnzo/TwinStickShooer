using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public Animator animator;


    public void Enable()
    {
        animator.SetBool("isOpen", true);
    }

    public void Disable()
    {
        animator.SetBool("isOpen", false);
    }
}
