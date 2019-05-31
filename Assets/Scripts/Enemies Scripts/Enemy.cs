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
    public bool walking = true;
    private bool preparingToFire = false;
    private bool isKnockedBack = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
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

    public void OnTriggerEnter2D(Collider2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "PlayerBullet" && gameObject.tag == "Enemy")
        {
            isKnockedBack = true;
            Vector2 force = collidingObject.GetComponent<PlayerBullet>().getBulletTrajectory();
            force.Normalize();
            GetComponent<Rigidbody2D>().AddForce(force * collidingObject.GetComponent<PlayerBullet>().getBulletKnockBack());
        }
    }
    private void followPlayer(float distFromPlayer)
    {
        if (!preparingToFire && !isKnockedBack)
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
