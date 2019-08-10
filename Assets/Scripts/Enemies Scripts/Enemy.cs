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
    public bool isSpawned = false;
    public float knockBack = 5f;
    public Vector2 enemyTrajectory;
    public float startingHealth = 3f;
    public float health;
    public float waitBeforeFire = 1f;
    public float stopAndFireRange = 7f;
    public float walkAndFireRange = 9f;
    public float collideDamageToPlayer = 2f;
    public float moveSpeed = 5f;
    public bool preparingToFire = false;
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
        activateStateMachine();
    }

    protected void OnEnable()
    {
        StartOrEnableEnemy();
        activateStateMachine();
    }

    protected void activateStateMachine()
    {
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.ChangeState(StatePlayerFollow.Instance);
    }

    private void StartOrEnableEnemy()
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
    protected void Update()
    {
        //Tracks floating text number locations relative to the enemy
        TrackFloatingTextPos();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        enemyTrajectory = rb.velocity;
        stateMachine.Update();
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
        enemyTrajectory = Vector2.zero;
        if (gameObject.activeInHierarchy == true)
        {
            aiPath.canMove = false;
            Vector2 difference = knockBackTrajectory;
            difference = difference.normalized * knockBackForce;
            rb.AddForce(difference, ForceMode2D.Impulse);
        }
        if (health <= 0f)
        {
            stateMachine.ChangeState(StateDie.Instance);
            stateMachine.Update();
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
}
