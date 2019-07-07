using System.IO;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Enemy : MonoBehaviour
{
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
    private float coolDownOnMovementTimer = .5f;
    private float movementCoolDownReset = .5f;
    private List<FloatingText> floatingText;

    void OnEnable()
    {
        floatingText = new List<FloatingText>();
        rb = GetComponent<Rigidbody2D>();
        aiPath.canMove = false;
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = Player.Instance.transform;
        health = startingHealth;
    }

    void OnDisable()
    {
        TrackFloatingTextPos();
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            collisionInfo.gameObject.GetComponent<Player>().hit(collideDamageToPlayer);
        }
    }
    public void hit(float Damage, float knockBackForce, Vector2 knockBackTrajectory)
    {
        FloatingText floatingDamageText = FloatingTextController.CreateFloatingText(Damage.ToString(), transform);
        floatingText.Add(floatingDamageText);
        health -= Damage;
        if (gameObject.activeInHierarchy == true)
        {
            aiPath.canMove = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            Vector2 difference = knockBackTrajectory;
            difference = difference.normalized * knockBackForce;
            rb.AddForce(difference, ForceMode2D.Impulse);
        }
        if (health <= 0f)
        {
            die();
        }
    }

    void Update()
    {
        //Tracks floating text number locations relative to the enemy
        TrackFloatingTextPos();

        //CoolDown for Movement after being knocked back
        if (!aiPath.canMove)
        {
            coolDownOnMovementTimer -= Time.deltaTime;
            if (coolDownOnMovementTimer <= 0)
            {
                aiPath.canMove = true;
                coolDownOnMovementTimer = movementCoolDownReset;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (aiPath.canMove == true)
        {
            enemyTrajectory = rb.velocity;
            float distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
            followPlayer(distFromPlayer);
            shootAtPlayer(distFromPlayer);
        }
    }
    private void followPlayer(float distFromPlayer)
    {
        if (!preparingToFire)
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
    private void die()
    {
        //Play some animation, particles, and sounds
        dropCraftComponents();
        enemyKilled.Raise();
        gameObject.SetActive(false);

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
    private void TrackFloatingTextPos()
    {
        List<FloatingText> removeText = new List<FloatingText>();
        foreach (FloatingText floatingText in floatingText)
        {
            if (floatingText != null)
            {
                FloatingTextController.SetFloatingTextLocation(floatingText, transform);
            }
            else
            {
                removeText.Add(floatingText);
            }
        }
        foreach (FloatingText removableText in removeText)
        {
            if (floatingText.Contains(removableText))
            {
                floatingText.Remove(removableText);
            }
        }
    }
}
