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
    public float stopAndFireRange = 7f;
    public float walkAndFireRange = 9f;
    public float collideDamageToPlayer = 2f;
    public float moveSpeed = 5f;
    public bool walking = true;
    private bool preparingToFire = false;
    public bool isKnockedBack = false;
    private Rigidbody2D rb;
    public AIPath aiPath;
    private AIDestinationSetter AIDestinationSetter;

    //take damage variables
    public Signal enemyKilled;
    public GameObject greenCraftComponent;
    public GameObject purpleCraftComponent;
    public GameObject blackCraftComponent;
    public int minDropCount = 0;
    public int maxDropCount = 7;
    public float minDropDist = 2f;
    public float maxDropDist = 2f;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        aiPath.canMove = false;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = Player.Instance.transform;
        health = startingHealth;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            collisionInfo.gameObject.GetComponent<Player>().hit(collideDamageToPlayer, knockBack, enemyTrajectory);
        }
    }
    public void hit(float Damage, float knockBack, Vector2 knockBackTrajectory)
    {
        health -= Damage;
        if (gameObject.activeInHierarchy == true)
        {
            isKnockedBack = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 difference = knockBackTrajectory;
            difference = difference.normalized * knockBack;
            rb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(knockCo(rb));
        }
        // StartCoroutine(knockCo(1f, knockBack, knockBackTrajectory));
        if (health <= 0f)
        {
            die();
        }
    }
    private void die()
    {
        if (health <= 0f)
        {
            //Play some animation, particles, and sounds
            dropCraftComponents();
            enemyKilled.Raise();
            gameObject.SetActive(false);
        }
    }
    private IEnumerator knockCo(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
    }

    private void dropCraftComponents()
    {
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(greenCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(purpleCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(blackCraftComponent.name + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = this.transform.rotation;
                newComponent.SetActive(true);
            }
        }
    }

    private float randomDistFromEnemy(float pos)
    {
        return Random.Range(pos - minDropDist, pos + maxDropDist);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkIfKnockedBack();
        enemyTrajectory = rb.velocity;
        float distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
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
            if (distFromPlayer <= stopAndFireRange)
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
        if (!preparingToFire && distFromPlayer <= walkAndFireRange && distFromPlayer > stopAndFireRange)
        {
            GetComponentInChildren<EnemyGun>().EnemyFireGun();
        }
        if (!preparingToFire && distFromPlayer <= stopAndFireRange)
        {
            StartCoroutine(takeAimThenFire());
        }
    }

    IEnumerator takeAimThenFire()
    {
        preparingToFire = true;
        yield return new WaitForSeconds(waitBeforeFire);
        GetComponentInChildren<EnemyGun>().EnemyFireGun();
        preparingToFire = false;
    }
}
