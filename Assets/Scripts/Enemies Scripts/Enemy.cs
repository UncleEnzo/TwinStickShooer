using System.IO;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public float knockTime = .25f;
    public float knockBack = 5f;
    public Vector2 enemyTrajectory;
    public float startingHealth = 3f;
    public float health;
    public float waitBeforeFire = 1f;
    public float firingRange = 5f;
    public float collideDamageToPlayer = 2f;
    public float moveSpeed = 5f;
    private Player player;
    public bool walking = true;
    private bool preparingToFire = false;
    public bool isKnockedBack = false;
    private Rigidbody2D rb;
    public AIPath aiPath;
    private AIDestinationSetter AIDestinationSetter;

    void OnEnable()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        aiPath.canMove = false;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = player.transform;
        health = startingHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkIfKnockedBack();
        enemyTrajectory = rb.velocity;
        float distFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        followPlayer(distFromPlayer);
        shootAtPlayer(distFromPlayer);
    }

    private void checkIfKnockedBack()
    {
        Vector2 canMove = new Vector2(0f, 0f);
        if (GetComponent<Rigidbody2D>().velocity == canMove)
        {
            isKnockedBack = false;
        }
    }
    private void followPlayer(float distFromPlayer)
    {
        if (!preparingToFire && !isKnockedBack)
        {
            if (distFromPlayer <= firingRange)
            {
                aiPath.canMove = false;
            }
            else
            {
                aiPath.canMove = true;
            }
        }
        else
        {
            aiPath.canMove = false;
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
}
