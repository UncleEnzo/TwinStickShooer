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
    Die
}
public class Enemy : MonoBehaviour
{
    [Header("Note: Move speed is set in AiPath. It's called Max Speed")]
    public bool isSpawned = false;
    public float knockBack = 5f;
    public Vector2 enemyTrajectory;
    public float startingHealth = 3f;
    public float health;
    public float waitBeforeFire = 1f;
    public float stopAndFireRange = 7f;
    public float walkAndFireRange = 9f;
    public float collideDamageToPlayer = 2f;
    public bool knockedBack = false;
    [System.NonSerialized]
    public Rigidbody2D rb;
    public AIPath aiPath;
    [System.NonSerialized]
    public AIDestinationSetter AIDestinationSetter;

    //take damage variables
    public Signal enemyKilled;
    public GameObject greenCraftComponent;
    public GameObject purpleCraftComponent;
    public GameObject blackCraftComponent;
    public GameObject key;
    public int minDropCount = 0;
    public int maxDropCount = 7;
    public float minDropDist = 2f;
    public float maxDropDist = 2f;
    public float coolDownOnMovementTimer = .5f;
    public float movementCoolDownReset = .5f;
    public List<FloatingText> floatingText;
    public StateMachine<Enemy> stateMachine { get; set; }
    public EnemyStates enemyStates;
    public float distFromPlayer;
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
        floatingText = new List<FloatingText>();
        rb = GetComponent<Rigidbody2D>();
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        AIDestinationSetter.target = Player.Instance.gameObject.transform;
        health = startingHealth;
        enemyStates = EnemyStates.FollowPlayer;
    }

    protected void OnDisable()
    {
        TrackFloatingTextPos();
        aiPath.canMove = true;
    }
    protected void Update()
    {
        //Tracks floating text number locations relative to the enemy
        TrackFloatingTextPos();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        enemyTrajectory = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            collisionInfo.gameObject.GetComponent<Player>().hit(collideDamageToPlayer);
        }
    }
    public virtual void hit(float Damage, float knockBackForce, Vector2 knockBackTrajectory)
    {
        FloatingText floatingDamageText = FloatingTextController.CreateFloatingText(Damage.ToString(), transform);
        floatingText.Add(floatingDamageText);
        health -= Damage;
        enemyTrajectory = Vector2.zero;
        if (gameObject.activeInHierarchy == true)
        {
            aiPath.canMove = false;
            knockedBack = true;
            Vector2 difference = knockBackTrajectory;
            difference = difference.normalized * knockBackForce;
            rb.AddForce(difference, ForceMode2D.Impulse);
        }
        if (health <= 0f)
        {
            //Play some animation, particles, and sounds
            dropCraftComponents();
            dropKey();
            if (isSpawned)
            {
                isSpawned = false;
                enemyKilled.Raise();
            }
            if (GetComponent<Gun>())
            {
                GetComponent<Gun>().currentAmmo = GetComponent<Weapon>().GunProperties.maxAmmo;
            }
            gameObject.SetActive(false);
        }
    }

    protected void knockBackAction()
    {
        //CoolDown for Movement after being knocked back
        if (knockedBack)
        {
            coolDownOnMovementTimer -= Time.deltaTime;
            if (coolDownOnMovementTimer <= 0)
            {
                knockedBack = false;
                aiPath.canMove = true;
                rb.velocity = Vector2.zero;
                coolDownOnMovementTimer = movementCoolDownReset;
            }
        }
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
    private void dropCraftComponents()
    {
        enableComponents(greenCraftComponent.name);
        enableComponents(purpleCraftComponent.name);
        enableComponents(blackCraftComponent.name);
    }
    private void enableComponents(string craftComponentName)
    {
        for (int i = 0; i < Random.Range(minDropCount, maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(craftComponentName + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newComponent.transform.rotation = transform.rotation;
                newComponent.SetActive(true);
            }
        }
    }
    private void dropKey()
    {
        int keyDropCheck = Random.Range(0, 10);
        if (keyDropCheck == 1)
        {
            GameObject newKey = ObjectPooler.SharedInstance.GetPooledObject(key.name + "(Clone)");
            if (newKey != null)
            {
                newKey.transform.position = new Vector2(randomDistFromEnemy(transform.position.x), randomDistFromEnemy(transform.position.y));
                newKey.transform.rotation = transform.rotation;
                newKey.SetActive(true);
            }
        }
    }
    private float randomDistFromEnemy(float pos)
    {
        return Random.Range(pos - minDropDist, pos + maxDropDist);
    }
}
