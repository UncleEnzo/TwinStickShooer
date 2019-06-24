using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerSavedData localPlayerData = new PlayerSavedData();
    public Player player;
    private PlayerHUBController playerHUB;
    private float healthDefault = 8f;

    [Header("IFrames")]
    public Rigidbody2D myRigidBody;
    public BoxCollider2D triggerCollider;
    public GameObject movementAnimation;
    public SpriteRenderer mySprite;
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public bool iFramesActive = false;

    public void Start()
    {
        PersistentGameData persistentGameData = FindObjectOfType<PersistentGameData>();
        playerHUB = FindObjectOfType<PlayerHUBController>();
        if (persistentGameData.currentHealth > 0f)
        {
            localPlayerData.health = persistentGameData.currentHealth;
        }
        else
        {
            localPlayerData.health = healthDefault;
        }
        playerHUB.updateDisplayHubHealth(localPlayerData.health);
    }

    public void OnTriggerEnter2D(Collider2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "EnemyBullet" && gameObject.tag == "Player")
        {
            collidingObject.gameObject.SetActive(false);
        }
        takeDamage(collidingObject);
        die();
    }

    public void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (iFramesActive == true && collisionInfo.gameObject.tag == "EnemyBullet")
        {
            collisionInfo.gameObject.SetActive(false);
        }
    }
    private void die()
    {
        if (localPlayerData.health <= 0f)
        {
            FindObjectOfType<SceneLoader>().loadGameOverScene();
        }
    }
    private void takeDamage(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            localPlayerData.health -= collision.gameObject.GetComponent<EnemyBullet>().bulletDamage;
            playerHUB.updateDisplayHubHealth(localPlayerData.health);
            if (iFramesActive == false)
            {
                StartCoroutine(KnockCo(collision.gameObject.GetComponent<EnemyBullet>().knockTime, collision.gameObject.GetComponent<EnemyBullet>().knockBack, collision.gameObject.GetComponent<EnemyBullet>().bulletTrajectory));
            }

        }
        if (collision.gameObject.tag == "Enemy")
        {
            localPlayerData.health -= collision.gameObject.GetComponent<Enemy>().collideDamageToPlayer;
            playerHUB.updateDisplayHubHealth(localPlayerData.health);
            if (iFramesActive == false)
            {
                StartCoroutine(KnockCo(collision.gameObject.GetComponent<Enemy>().knockTime, collision.gameObject.GetComponent<Enemy>().knockBack, collision.gameObject.GetComponent<Enemy>().enemyTrajectory));
            }

        }
    }

    private IEnumerator KnockCo(float knockTime, float knockBack, Vector2 trajectory)
    {
        if (myRigidBody != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 difference = trajectory;
            difference = difference.normalized * knockBack;
            rb.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
        }
    }

    private IEnumerator FlashCo()
    {
        iFramesActive = true;
        int temp = 0;
        triggerCollider.enabled = false;
        movementAnimation.SetActive(false);
        mySprite.enabled = true;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        mySprite.enabled = false;
        movementAnimation.SetActive(true);
        triggerCollider.enabled = true;
        iFramesActive = false;
    }
}