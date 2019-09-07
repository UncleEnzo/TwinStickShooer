using System.IO;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using StateMachine;

public enum EnemyStates
{
    FollowPlayer,
    MoveShoot,
    StopShoot,
    KnockedBack,
    Die
}
public class Enemy : MonoBehaviour
{
    Coroutine lastCoroutine = null;
    private Collider2D Collider;
    [Header("Note: Move speed is set in AiPath. It's called Max Speed")]
    public bool isSpawned = false;
    public float knockBackTimer = .2f;
    public Vector2 enemyTrajectory;
    public float startingHealth = 3f;
    public float health;
    protected bool preparingToFire = false;
    public float waitBeforeFire = 2f;
    public float stopAndFireRange = 7f;
    public float walkAndFireRange = 9f;
    public float collideDamageToPlayer = 2f;
    public bool knockedBack = false;
    public AIPath aiPath;
    [Header("Sound Effects")]
    public AudioClip enemyHitSound;
    public AudioClip enemyDeathSound;
    private AudioSource enemySounds;
    [Header("IFrames")]
    #region IFrames
    private SpriteRenderer sprite;
    private Color flashColor = new Color(255, 255, 255, 65);
    private Color regularColor;
    private float flashDuration = 0.07f;
    private int numberOfFlashes = 1;
    #endregion IFrames
    [System.NonSerialized]
    public Rigidbody2D rb;
    [System.NonSerialized]
    public AIDestinationSetter AIDestinationSetter;

    //take damage variables
    public Signal enemyKilled;
    protected EnemyGun enemyGun;
    public bool destroyBulletsOnDeath = true;
    public GameObject greenCraftComponent;
    public GameObject purpleCraftComponent;
    public GameObject blackCraftComponent;
    public GameObject key;
    public int PercentDropChance = 90;
    public int NumOfRolls = 7;
    public float coolDownOnMovementTimer = .5f;
    public float movementCoolDownReset = .5f;
    private List<FloatingText> floatingTextList;
    private List<FloatingText> removeTextList;
    public StateMachine<Enemy> stateMachine { get; set; }
    public EnemyStates enemyState;
    public float distFromPlayer;
    public float enemyCorpseTimer = 10f;
    public GameObject deadEnemyMarker;
    private bool m_isQuitting;
    protected void Start()
    {
        StartOrEnableEnemy();
    }

    protected void OnEnable()
    {
        if (Player.Instance)
        {
            StartOrEnableEnemy();
        }
    }

    private void StartOrEnableEnemy()
    {
        Collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        regularColor = sprite.color;
        enemySounds = GetComponent<AudioSource>();
        deadEnemyMarker.SetActive(false);
        floatingTextList = new List<FloatingText>();
        removeTextList = new List<FloatingText>();
        rb = GetComponent<Rigidbody2D>();
        enemyGun = GetComponentInChildren<EnemyGun>();
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = Player.Instance.gameObject.transform;
        health = startingHealth;
        enemyState = EnemyStates.FollowPlayer;
    }

    protected virtual void OnApplicationQuit()
    {
        m_isQuitting = true;
    }

    protected void OnDisable()
    {
        if (!m_isQuitting && Application.isPlaying == true)
        {
            TrackFloatingTextPos();
        }
        aiPath.canMove = true;
    }
    protected void Update()
    {
        //Tracks floating text number locations relative to the enemy
        if (!m_isQuitting && Application.isPlaying == true)
        {
            TrackFloatingTextPos();
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        enemyTrajectory = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (enemyState != EnemyStates.Die)
        {
            if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
            {
                collisionInfo.gameObject.GetComponent<Player>().hit(collideDamageToPlayer);
            }
        }
    }
    public virtual void hit(float Damage, float knockBackForce,
        Vector2 knockBackTrajectory, bool showDamageText = true)
    {
        if (enemyState != EnemyStates.Die)
        {
            if (showDamageText)
            {
                FloatingText floatingDamageText = FloatingTextController.CreateFloatingText(Damage.ToString(), transform);
                floatingTextList.Add(floatingDamageText);
            }
            enemySounds.PlayOneShot(enemyHitSound);
            health -= Mathf.Round(Damage);
            if (health <= 0f)
            {
                //Stops enemy logic in this script
                enemyState = EnemyStates.Die;

                dropCraftComponentsApplyPush(PercentDropChance, NumOfRolls);
                dropLoot(key.name, 20, 1);

                if (isSpawned)
                {
                    isSpawned = false;
                    enemyKilled.Raise();
                }
                if (GetComponent<Gun>())
                {
                    GetComponent<Gun>().setCurrentAmmo(GetComponent<Weapon>().GunProperties.maxAmmo);
                }
                //Play Death Animation >> Need to switch death marker to animation up here 
                deadEnemyMarker.SetActive(true);
                StopAllCoroutines();
                //note: give the enemy a ragdoll motion upon death.  Want to increase linear drag to compensate
                rb.AddForce(knockBackTrajectory.normalized * knockBackForce, ForceMode2D.Impulse);
                StartCoroutine(enemyDeath());
            }
            else
            {
                if (Damage > 0)
                {
                    StartCoroutine(FlashCo());
                }
                lastCoroutine = StartCoroutine(knockedBackCo(knockBackForce, knockBackTrajectory));
            }
        }
    }

    private IEnumerator FlashCo()
    {
        //NOTE: When you get animations for enemies, mimic the method in player
        int temp = 0;
        while (temp < numberOfFlashes)
        {
            sprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            sprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
    }

    IEnumerator enemyDeath()
    {
        //Play Death sound effect
        if (destroyBulletsOnDeath)
        {
            if (enemyGun != null)
            {
                enemyGun.shotControllerShowCase.activeShotCtrl.StopShotRoutineAndPlayingShot();
            }
            else
            {
                UbhShotCtrl[] shotControllers = GetComponentsInChildren<UbhShotCtrl>();
                foreach (UbhShotCtrl ubhShotCtrl in shotControllers)
                {
                    ubhShotCtrl.StopShotRoutineAndPlayingShot();
                }
            }
        }
        sprite.color = regularColor;
        gameObject.tag = TagsAndLabels.UntaggedTag;
        gameObject.layer = LayerMask.NameToLayer(TagsAndLabels.DeadEnemyLabel);
        SwitchVitals();
        deadEnemyMarker.SetActive(true);
        yield return new WaitForSeconds(enemyCorpseTimer);
        SwitchVitals();
        gameObject.tag = TagsAndLabels.EnemyTag;
        gameObject.layer = LayerMask.NameToLayer(TagsAndLabels.EnemyLabel);
        deadEnemyMarker.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator knockedBackCo(float knockBackForce, Vector2 knockBackTrajectory)
    {
        enemyState = EnemyStates.KnockedBack;
        aiPath.canMove = false;
        Vector2 difference = knockBackTrajectory.normalized * knockBackForce;
        rb.AddForce(difference, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackTimer);
        rb.velocity = Vector2.zero;
        aiPath.canMove = true;
        enemyState = EnemyStates.FollowPlayer;
    }
    private void SwitchVitals()
    {
        aiPath.enabled = !aiPath.enabled;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.SetActive(!child.activeInHierarchy);
        }
    }
    private void TrackFloatingTextPos()
    {
        foreach (FloatingText floatingText in floatingTextList)
        {
            if (floatingText.isActiveAndEnabled)
            {
                FloatingTextController.SetFloatingTextLocation(floatingText, transform);
            }
            else
            {
                removeTextList.Add(floatingText);
            }
        }
        foreach (FloatingText removedFloatingText in removeTextList)
        {
            floatingTextList.Remove(removedFloatingText);
        }
        removeTextList.Clear();
    }

    private void dropCraftComponentsApplyPush(int PercentDropChance, int NumOfRolls)
    {
        float push = .0005f;
        List<GameObject> GreenCraftComponents = dropLoot(greenCraftComponent.name, PercentDropChance, NumOfRolls);
        foreach (GameObject droppedItem in GreenCraftComponents)
        {
            droppedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-push, push), Random.Range(-push, push)), ForceMode2D.Impulse);
        }
        List<GameObject> PurpleCraftComponents = dropLoot(purpleCraftComponent.name, PercentDropChance, NumOfRolls);
        foreach (GameObject droppedItem in PurpleCraftComponents)
        {
            droppedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-push, push), Random.Range(-push, push)), ForceMode2D.Impulse);
        }
        List<GameObject> ExplosiveCraftComponents = dropLoot(blackCraftComponent.name, PercentDropChance, NumOfRolls);
        foreach (GameObject droppedItem in ExplosiveCraftComponents)
        {
            droppedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-push, push), Random.Range(-push, push)), ForceMode2D.Impulse);
        }
    }

    //PercentChanceDrop: Percent Drop chance out of 100
    private List<GameObject> dropLoot(string ItemName, int PercentChanceDrop, int NumOfRolls)
    {
        List<GameObject> DroppedLoot = new List<GameObject>();
        for (int i = 0; i < NumOfRolls; i++)
        {
            if (Random.Range(0, 100) <= PercentChanceDrop)
            {
                GameObject droppedItem = ObjectPooler.SharedInstance.GetPooledObject(ItemName + "(Clone)");
                if (droppedItem != null)
                {
                    droppedItem.transform.position = new Vector2(transform.position.x, transform.position.y);
                    droppedItem.transform.rotation = transform.rotation;
                    droppedItem.SetActive(true);
                    DroppedLoot.Add(droppedItem);
                }
            }
        }
        return DroppedLoot;
    }
}
