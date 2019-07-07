using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator animator;
    private Text damageText;
    public float randomPosMovementX;
    public float randomPosMovementY;

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damageText = animator.GetComponent<Text>();
        randomPosMovementX = Random.Range(-.4f, .4f);
        randomPosMovementY = Random.Range(-.4f, .4f);
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }
}