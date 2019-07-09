using System.IO;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using StateMachine;

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
    public bool preparingToFire = false;

    public Rigidbody2D rb;
    public AIPath aiPath;
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
    public bool switchState = false;

    //Note: Experimental call so that enemies can be dragged in from prefabs as well as Enabled > If it works, make it a private method that's called from both
    protected void Start()
    {
        StartOrEnableEnemy();
        stateMachine = new StateMachine<Enemy>(this);
        stateMachine.ChangeState(StatePlayerFollow.Instance);
    }

    protected void OnEnable()
    {
        StartOrEnableEnemy();
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
    protected void FixedUpdate()
    {

        if (aiPath.canMove == true)
        {
            enemyTrajectory = rb.velocity;

            //FOLLOW PLAYER STATE
            stateMachine.ChangeState(StatePlayerFollow.Instance);
            stateMachine.Update();

            //SHOOT AT PLAYER STATE
            stateMachine.ChangeState(StateMoveShoot.Instance);
            stateMachine.Update();

        }
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
        if (gameObject.activeInHierarchy == true)
        {
            //ENTER KNOCKBACK STATE, DON"T JUST CALL IT
            aiPath.canMove = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
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
