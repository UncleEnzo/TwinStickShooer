using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public float reduceMassBy = .75f;
    public float reduceScaleBy = 1f;
    private float defaultSpeed;
    private Vector2 defaultScale;
    private float defaultMass;

    new private void Start()
    {
        base.Start();
        SetMeleeEnemyDefaultValues();
    }

    new private void OnEnable()
    {
        base.OnEnable();
        SetMeleeEnemyDefaultValues();
    }

    private void SetMeleeEnemyDefaultValues()
    {
        aiPath.canMove = true;
        defaultSpeed = aiPath.maxSpeed;
        defaultScale = transform.localScale;
        //Note: Needs to be set like this instead of using variable because it is not yet found on start up
        defaultMass = GetComponent<Rigidbody2D>().mass;
    }

    new private void OnDisable()
    {
        base.OnDisable();
        aiPath.canMove = true;
        aiPath.maxSpeed = defaultSpeed;
        transform.localScale = new Vector2(defaultScale.x, defaultScale.y);
        rb.mass = defaultMass;
    }

    public override void hit(float Damage, float knockBackForce, Vector2 knockBackTrajectory, bool showDamageText = true)
    {
        if (aiPath.maxSpeed < 10)
        {
            aiPath.maxSpeed++;
        }
        if (transform.localScale.x > 2)
        {
            if ((transform.localScale.x - reduceScaleBy) > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x - reduceScaleBy, transform.localScale.y - reduceScaleBy);
            }
        }
        if (rb.mass > 1.5) //min can't be less than 0 at the end of this equation
        {
            if ((rb.mass - reduceMassBy) > 0)
            {
                rb.mass -= reduceMassBy;
            }
        }
        base.hit(Damage, knockBackForce, knockBackTrajectory, showDamageText);
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        knockBackAction();
    }
}
