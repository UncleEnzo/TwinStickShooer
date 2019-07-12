using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

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
    public float speed = 10f;
    public Rigidbody2D myRigidBody;
    public PlayerAnimController animator;
    public bool canMove = true;
    private float coolDownOnMovementTimer = 1f;
    private float movementCoolDownReset = 1f;
    public bool playerUsable = true;
    public float health = 8f;

    [Header("IFrames")]
    #region IFrames
    public BoxCollider2D triggerCollider;
    public GameObject movementAnimation;
    public SpriteRenderer mySprite;
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public bool iFramesActive = false;
    #endregion
    public void Start()
    {
        SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
        health = SavePersistentData.health;
        PlayerHUBController.Instance.updateDisplayHubHealth(health);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //CoolDown for Movement after being knocked back
        if (!canMove)
        {
            coolDownOnMovementTimer -= Time.deltaTime;
            if (coolDownOnMovementTimer <= 0)
            {
                canMove = true;
                coolDownOnMovementTimer = movementCoolDownReset;
            }
        }
        if (canMove)
        {
            Move();
        }
    }

    public void Move()
    {
        Vector3 tempVect = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        myRigidBody.MovePosition(transform.position + tempVect);
    }
    public void enablePlayer(Boolean playerUsable)
    {
        this.playerUsable = playerUsable;
        CameraController.Instance.enabled = playerUsable;
        CursorController.Instance.enabled = playerUsable;
        WeaponSwitching.Instance.GetComponent<SetGunPosition>().enabled = playerUsable;
        Transform currentWeapon = WeaponSwitching.Instance.getSelectedWeapon();
        currentWeapon.GetComponentInChildren<Weapon>().enabled = playerUsable;
        animator.enabled = playerUsable;
        canMove = playerUsable;
    }
    public void hit(float Damage)
    {
        hit(Damage, 0, new Vector2(0, 0));
    }
    public void hit(float Damage, float knockbackForce, Vector2 knockBackTrajectory)
    {
        if (knockbackForce != 0)
        {
            canMove = false;
        }
        if (!iFramesActive)
        {
            health -= Damage;
        }
        PlayerHUBController.Instance.updateDisplayHubHealth(health);
        if (!iFramesActive)
        {
            if (knockbackForce != 0)
            {
                StartCoroutine(KnockCo(1f, knockbackForce, knockBackTrajectory));
            }
            if (knockbackForce == 0)
            {
                StartCoroutine(FlashCo());
            }
        }
        if (health <= 0f)
        {
            die();
        }
    }
    private void die()
    {
        SceneLoader.loadGameOverScene();
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
