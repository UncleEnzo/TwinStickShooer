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
        float timer = clipInfo[0].clip.length;
        damageText = animator.GetComponent<Text>();
        StartCoroutine(destroyCo(timer));
        randomPosMovementX = Random.Range(-.4f, .4f);
        randomPosMovementY = Random.Range(-.4f, .4f);
    }

    private IEnumerator destroyCo(float timer)
    {
        //Note: Lowering by point one so that it disables before the animation gets a chance to restart
        yield return new WaitForSeconds(timer - .1f);
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }
}