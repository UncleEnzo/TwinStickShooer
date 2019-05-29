using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float waitBeforeFire = 1f;
    public float firingRange = 5f;
    public float collideDamageToPlayer = 2f;
    public float moveSpeed;
    private Player player;
    private bool walking = true;
    private bool preparingToFire = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        followPlayer(distFromPlayer);
        shootAtPlayer(distFromPlayer);
    }

    private void followPlayer(float distFromPlayer)
    {
        if (!preparingToFire)
        {
            if (distFromPlayer <= firingRange)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }
            if (walking)
            {
                approachPlayer();
            }
        }
    }

    private void shootAtPlayer(float distFromPlayer)
    {
        if (!preparingToFire && distFromPlayer <= firingRange)
        {
            StartCoroutine(takeAimThenFire());
        }
    }

    IEnumerator takeAimThenFire()
    {
        preparingToFire = true;
        yield return new WaitForSeconds(waitBeforeFire);
        GetComponentInChildren<EnemyGunFire>().fireEnemyGun();
        preparingToFire = false;
    }

    private void approachPlayer()
    {
        Vector3 destination = player.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}
