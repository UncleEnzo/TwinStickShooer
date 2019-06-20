using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerSavedData localPlayerData = new PlayerSavedData();
    private PlayerHUBController playerHUB;
    private float healthDefault = 8f;

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

    public void OnCollisionEnter2D(Collision2D collidingObject)
    {
        if (collidingObject.gameObject.tag == "EnemyBullet" && gameObject.tag == "Player")
        {
            collidingObject.gameObject.SetActive(false);
        }
        takeDamage(collidingObject);
        die();
    }
    private void die()
    {
        if (localPlayerData.health <= 0f)
        {
            FindObjectOfType<SceneLoader>().loadGameOverScene();
        }
    }
    private void takeDamage(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            localPlayerData.health -= collision.gameObject.GetComponent<EnemyBullet>().bulletDamage;
            playerHUB.updateDisplayHubHealth(localPlayerData.health);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            localPlayerData.health -= collision.gameObject.GetComponent<Enemy>().collideDamageToPlayer;
            playerHUB.updateDisplayHubHealth(localPlayerData.health);
        }
    }
}