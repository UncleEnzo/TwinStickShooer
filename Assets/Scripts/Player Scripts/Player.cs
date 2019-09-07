using System.Linq;
using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStates
{
    MovingShooting,
    Disabled,
    KnockedBack,
    Die
}
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion
    public PlayerStates playerState;
    public float speed = 10f;
    public Rigidbody2D rb;
    public PlayerAnimController animator;
    private float damagedKnockBackForce = 13f;
    private float damagedKnockBackRadius = 5f;
    private GameObject reloadUIObject;
    private float knockedBackTimer = .5f;
    public bool playerUsable = true;
    public float totalHealth = 8f;
    public float health = 8f;

    [Header("IFrames")]
    #region IFrames
    public BoxCollider2D triggerCollider;
    public GameObject movementAnimation;
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    private bool iFramesActive = false;
    #endregion
    [Header("Sound Effects")]
    public AudioClip hitSound;
    private AudioSource playerSounds;


    public void Start()
    {
        reloadUIObject = GameObject.Find("Canvas").transform.Find("ReloadSlider").gameObject;
        reloadUIObject.SetActive(false);
        playerSounds = GetComponent<AudioSource>();
        playerState = PlayerStates.MovingShooting;
        if (SceneManager.GetActiveScene().buildIndex != SceneLoader.hubWorldIndex)
        {
            SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
            if (SavePersistentData != null)
            {
                health = SavePersistentData.health;
                totalHealth = SavePersistentData.totalHealth;
            }
            else
            {
                health = totalHealth;
            }
            PlayerHUBController.Instance.updateDisplayHubHealth(health, totalHealth);
        }
        else
        {
            health = totalHealth;
            PlayerHUBController.Instance.updateDisplayHubHealth(health, totalHealth);
        }
    }

    void FixedUpdate()
    {
        UpdateReloadBarLocation();
        //Note:Feels weird but if you ever decide to use this... I'm leaving it here
        // if (playerState == PlayerStates.MovingShooting
        //     && !Input.GetKey(KeyCode.W)
        //     && !Input.GetKey(KeyCode.A)
        //     && !Input.GetKey(KeyCode.S)
        //     && !Input.GetKey(KeyCode.D))
        // {
        //     rb.velocity = Vector2.zero;
        //     animator.topAnimator.StopPlayback();
        //     animator.bottomAnimator.StopPlayback();
        // }
        if (playerState == PlayerStates.MovingShooting)
        {
            Move();
        }
    }

    private void UpdateReloadBarLocation()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + 1f));
        reloadUIObject.transform.position = screenPosition;
    }

    public void Move()
    {
        Vector3 tempVect = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
    }

    public void hit(float Damage)
    {
        hit(Damage, 0, new Vector2(0, 0));
    }
    public void hit(float Damage, float knockbackForce, Vector2 knockBackTrajectory)
    {
        if (!iFramesActive)
        {
            health -= Mathf.Round(Damage);
            PlayerHUBController.Instance.updateDisplayHubHealth(health, totalHealth, Damage);
        }
        if (!iFramesActive)
        {
            //knocks back surrounding enemies
            knockBackEnemies();

            if (InventoryUI.UIOpen)
            {
                InventoryUI.openOrCloseInventory();
            }

            //applies knockback to self
            if (knockbackForce != 0)
            {
                StartCoroutine(FlashCo());
                StartCoroutine(knockedBackCo(knockbackForce, knockBackTrajectory));
            }
            if (knockbackForce == 0)
            {
                playerSounds.PlayOneShot(hitSound);
                StartCoroutine(FlashCo());
            }
        }
        if (health <= 0f)
        {
            die();
        }
    }

    public void enablePlayer(Boolean playerUsableVar)
    {
        //note: Using this to disable gun firing (but keep reloading)
        //note: Using this to keep tracking gun pos but not have it update
        this.playerUsable = playerUsableVar;
        if (!playerUsableVar)
        {
            playerState = PlayerStates.Disabled;
        }
        else
        {
            playerState = PlayerStates.MovingShooting;
        }
        CameraController.Instance.enabled = playerUsableVar;
        CursorController.Instance.enabled = playerUsableVar;
        animator.enabled = playerUsableVar;
    }

    private void knockBackEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damagedKnockBackRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //disables enemy bullets caught in the knockBack effect
            if (nearbyObject.GetComponent<UbhBullet>()
                && nearbyObject.tag == TagsAndLabels.EnemyBulletTag)
            {
                nearbyObject.GetComponent<UbhBullet>().m_useAutoRelease = true;
                nearbyObject.GetComponent<UbhBullet>().m_autoReleaseTime = 1f;
                nearbyObject.GetComponent<UbhBullet>().m_selfTimeCount = 1f;
            }

            //Knocks Back Enemies And all other potential objects
            if (nearbyObject.tag != TagsAndLabels.PlayerBulletTag
                && nearbyObject.tag != TagsAndLabels.PlayerTag
                && nearbyObject.tag != TagsAndLabels.EnemyBulletTag
                && !nearbyObject.isTrigger
                && nearbyObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                Vector2 difference = rb.transform.position - transform.position;
                difference = difference * damagedKnockBackForce;
                if (rb.GetComponent<Enemy>())
                {
                    rb.GetComponent<Enemy>().hit(0, damagedKnockBackForce, difference, false);
                }
                else
                {
                    rb.AddForce(difference, ForceMode2D.Impulse);
                }
            }
        }
    }
    private void die()
    {
        SceneLoader.loadGameOverScene();
    }

    private IEnumerator knockedBackCo(float knockBackForce, Vector2 knockBackTrajectory)
    {
        if (rb != null)
        {
            playerState = PlayerStates.KnockedBack;
            Vector2 difference = knockBackTrajectory.normalized * knockBackForce;
            rb.AddForce(difference, ForceMode2D.Impulse);
            yield return new WaitForSeconds(knockedBackTimer);
            rb.velocity = Vector2.zero;
            playerState = PlayerStates.MovingShooting;
        }
    }
    private IEnumerator FlashCo()
    {
        iFramesActive = true;
        int temp = 0;
        triggerCollider.enabled = false;
        SpriteRenderer[] animationColors = GetComponentsInChildren<SpriteRenderer>();
        while (temp < numberOfFlashes)
        {
            foreach (SpriteRenderer animationColor in animationColors)
            {
                animationColor.color = flashColor;
            }
            yield return new WaitForSeconds(flashDuration);
            foreach (SpriteRenderer animationColor in animationColors)
            {
                animationColor.color = regularColor;
            }
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
        iFramesActive = false;
    }
}
