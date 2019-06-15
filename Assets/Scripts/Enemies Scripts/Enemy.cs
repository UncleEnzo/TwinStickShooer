using System.IO;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public float waitBeforeFire = 1f;
    public float firingRange = 5f;
    public float collideDamageToPlayer = 2f;
    public float knockTime = .25f;
    public float moveSpeed = 5f;
    private Player player;
    public bool walking = true;
    private bool preparingToFire = false;
    private bool isKnockedBack = false;
    private Rigidbody2D rb;
    public AIPath aiPath;
    private AIDestinationSetter AIDestinationSetter;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        aiPath.canMove = false;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = player.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkIfKnockedBack();
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

    public void OnCollisionEnter2D(Collision2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "PlayerBullet" && gameObject.tag == "Enemy")
        {
            isKnockedBack = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 difference = collidingObject.gameObject.GetComponent<PlayerBullet>().getBulletTrajectory();
            difference = difference.normalized * collidingObject.gameObject.GetComponent<PlayerBullet>().getBulletKnockBack();
            rb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(knockCo(rb));
        }
    }

    private IEnumerator knockCo(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
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
