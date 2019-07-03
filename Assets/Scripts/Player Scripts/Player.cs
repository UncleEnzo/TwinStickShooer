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
    public bool movementEnabled = true;
    private float coolDownOnMovement = 1f;
    public bool playerUsable = true;
    public static PlayerSavedData localPlayerData = new PlayerSavedData();
    private float healthDefault = 8f;

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
        PersistentGameData persistentGameData = PersistentGameData.Instance;
        if (persistentGameData.currentHealth > 0f)
        {
            localPlayerData.health = persistentGameData.currentHealth;
        }
        else
        {
            localPlayerData.health = healthDefault;
        }
        PlayerHUBController.Instance.updateDisplayHubHealth(localPlayerData.health);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //CoolDown for parry
        if (!movementEnabled)
        {
            coolDownOnMovement -= Time.deltaTime;
            print("Cooling down");
            if (coolDownOnMovement <= 0)
            {
                movementEnabled = true;
            }
        }
        if (movementEnabled)
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
        movementEnabled = playerUsable;
    }
    public void hit(float Damage, float knockBack, Vector2 knockBackTrajectory)
    {
        localPlayerData.health -= Damage;
        PlayerHUBController.Instance.updateDisplayHubHealth(localPlayerData.health);
        if (iFramesActive == false)
        {
            StartCoroutine(KnockCo(1f, knockBack, knockBackTrajectory));
        }
        if (localPlayerData.health <= 0f)
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
