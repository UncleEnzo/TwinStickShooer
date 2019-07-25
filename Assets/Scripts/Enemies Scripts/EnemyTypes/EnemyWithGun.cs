using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class EnemyWithGun : Enemy
{
    protected new void Update()
    {
        base.Update();

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
    protected new void FixedUpdate()
    {
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (!preparingToFire)
        {
            switch (enemyStates)
            {
                case EnemyStates.FollowPlayer:
                    if (aiPath.canMove == true)
                    {
                        stateMachine.ChangeState(StatePlayerFollow.Instance);
                    }
                    if (aiPath.canMove == true && distFromPlayer <= walkAndFireRange && distFromPlayer >= stopAndFireRange)
                    {
                        enemyStates = EnemyStates.MoveShoot;
                    }
                    break;
                case EnemyStates.MoveShoot:
                    if (aiPath.canMove == true)
                    {
                        stateMachine.ChangeState(StateMoveShoot.Instance);
                    }
                    if (aiPath.canMove == true && distFromPlayer >= walkAndFireRange)
                    {
                        enemyStates = EnemyStates.FollowPlayer;
                    }
                    else if (distFromPlayer <= stopAndFireRange)
                    {
                        enemyStates = EnemyStates.StopShoot;
                    }
                    break;
                case EnemyStates.StopShoot:
                    stateMachine.ChangeState(StateStopShoot.Instance);
                    if (aiPath.canMove == true && distFromPlayer <= walkAndFireRange && distFromPlayer >= stopAndFireRange)
                    {
                        enemyStates = EnemyStates.MoveShoot;
                    }
                    break;
            }
        }
        base.FixedUpdate();
    }
}
