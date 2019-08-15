using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class EnemyWithGun : Enemy
{
    protected new void Update()
    {
        base.Update();
        // knockBackAction();
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        switch (enemyState)
        {
            case EnemyStates.Die:
                break;
            case EnemyStates.FollowPlayer:
                //Case Switching
                if (distFromPlayer < walkAndFireRange && distFromPlayer > stopAndFireRange)
                {
                    enemyState = EnemyStates.MoveShoot;
                }
                if (distFromPlayer < stopAndFireRange)
                {
                    enemyState = EnemyStates.StopShoot;
                }
                //functionality of case > Inheritly the functionality is moving and nothing else
                if (!preparingToFire)
                {
                    aiPath.canMove = true;
                }
                break;
            case EnemyStates.MoveShoot:
                //Case Switching
                if (distFromPlayer > walkAndFireRange)
                {
                    enemyState = EnemyStates.FollowPlayer;
                }
                else if (distFromPlayer < stopAndFireRange)
                {
                    enemyState = EnemyStates.StopShoot;
                }
                //functionality of case
                enemyGun.EnemyFireGun();
                break;
            case EnemyStates.StopShoot:
                // StartCoroutine(takeAimThenFire());
                if (distFromPlayer > stopAndFireRange && distFromPlayer < walkAndFireRange)
                {
                    if (!preparingToFire)
                    {
                        aiPath.canMove = true;
                    }
                    enemyState = EnemyStates.MoveShoot;
                }
                if (distFromPlayer > walkAndFireRange)
                {
                    if (!preparingToFire)
                    {
                        aiPath.canMove = true;
                    }
                    enemyState = EnemyStates.FollowPlayer;
                }
                //functionality of case
                if (!preparingToFire)
                {
                    aiPath.canMove = false;
                    StartCoroutine(takeAimThenFire());
                }
                break;
        }
    }
    IEnumerator takeAimThenFire()
    {
        preparingToFire = true;
        yield return new WaitForSeconds(waitBeforeFire);
        enemyGun.EnemyFireGun();
        preparingToFire = false;
    }
}
