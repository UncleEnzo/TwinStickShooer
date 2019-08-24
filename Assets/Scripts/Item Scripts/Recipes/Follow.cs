using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform target;
    private float AttractorSpeed = 0;
    private Coroutine lastCoroutine = null;

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
        transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, AttractorSpeed * Time.deltaTime);
    }

    IEnumerator waitBeforeMagnetizing()
    {
        yield return new WaitForSeconds(1f);
        AttractorSpeed += Random.Range(1f, 1.5f);
        lastCoroutine = null;
    }
}
