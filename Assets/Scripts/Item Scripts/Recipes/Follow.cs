using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform target;
    public float minModifier = .1f;
    public float maxModifier = 2f;
    public float minWait = .3f;
    public float maxWait = 2f;

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance.transform;
        StartCoroutine(waitBeforeMagnetizing());
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            //May want to fix this by making it attracted to a target position a little in front of the direction the player is moving
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * (Random.Range(minModifier, maxModifier)));
        }
    }
    IEnumerator waitBeforeMagnetizing()
    {
        float waitTime = Random.Range(minWait, maxWait);
        yield return new WaitForSeconds(waitTime);
        isFollowing = true;
    }
}
