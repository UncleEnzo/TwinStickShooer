using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform target;
    private float AttractorSpeed = 0;
    private Coroutine lastCoroutine = null;
    private Rigidbody2D rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnDisable()
    {
        AttractorSpeed = 0f;
    }

    void FixedUpdate()
    {
        if (lastCoroutine == null)
        {
            StartCoroutine(waitBeforeMagnetizing());
        }

        //Todo > Expiration timer
        //Todo > flashing effect
        //todo > Only move towards you if you're close
        //todo only move towards you if you have l00  or less
        //Stop moving to you if you reach 100

        // transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, AttractorSpeed * Time.deltaTime);
    }

    IEnumerator waitBeforeMagnetizing()
    {
        yield return new WaitForSeconds(1f);
        if (AttractorSpeed < 15)
        {
            AttractorSpeed += Random.Range(1f, 1.5f);
        }
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        rb.velocity = direction * (AttractorSpeed);
        lastCoroutine = null;
    }
}
