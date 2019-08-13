using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class EnemyWithGun : Enemy
{
    protected new void Update()
    {
        base.Update();
        knockBackAction();
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (!knockedBack)
        {
            switch (enemyStates)
            {
                case EnemyStates.FollowPlayer:
                    //Case Switching
                    if (distFromPlayer < walkAndFireRange && distFromPlayer > stopAndFireRange)
                    {
                        enemyStates = EnemyStates.MoveShoot;
                    }
                    if (distFromPlayer < stopAndFireRange)
                    {
                        enemyStates = EnemyStates.StopShoot;
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
                        enemyStates = EnemyStates.FollowPlayer;
                    }
                    else if (distFromPlayer < stopAndFireRange)
                    {
                        enemyStates = EnemyStates.StopShoot;
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
                        enemyStates = EnemyStates.MoveShoot;
                    }
                    if (distFromPlayer > walkAndFireRange)
                    {
                        if (!preparingToFire)
                        {
                            aiPath.canMove = true;
                        }
                        enemyStates = EnemyStates.FollowPlayer;
                    }
                    //functionality of case
                    if (!preparingToFire)
                    {
                        aiPath.canMove = false;
                        StartCoroutine(takeAimThenFire());
                    }
                    break;
                case EnemyStates.Die:
                    break;
            }
        }
        base.FixedUpdate();
    }
    IEnumerator takeAimThenFire()
    {
        preparingToFire = true;
        yield return new WaitForSeconds(waitBeforeFire);
        enemyGun.EnemyFireGun();
        preparingToFire = false;
    }
}
