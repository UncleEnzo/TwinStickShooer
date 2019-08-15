using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaitAfterShot : Enemy
{
    private bool readyToFire = false;
    public float fireTimer = 2f;
    public float fireTimerReset = 2f;
    public float waitWhileShootTimer = 6f;
    public float waitWhileShootTimerReset = 6f;
    private bool readyToMove = true;

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (readyToMove)
        {
            switch (enemyState)
            {
                case EnemyStates.Die:
                    break;
                case EnemyStates.FollowPlayer:
                    //Case Switching
                    if (distFromPlayer < stopAndFireRange)
                    {
                        enemyState = EnemyStates.StopShoot;
                    }
                    //functionality of case > Inheritly the functionality is moving and nothing else
                    aiPath.canMove = true;
                    break;
                case EnemyStates.StopShoot:
                    if (distFromPlayer > stopAndFireRange)
                    {
                        aiPath.canMove = true;
                        enemyState = EnemyStates.FollowPlayer;
                    }
                    //functionality of case
                    aiPath.canMove = false;

                    //Shoot
                    if (distFromPlayer < stopAndFireRange)
                    {
                        //CoolDown for parry
                        if (!readyToFire)
                        {
                            fireTimer -= Time.deltaTime;
                            if (fireTimer <= 0)
                            {
                                readyToFire = true;
                                fireTimer = fireTimerReset;
                            }
                        }
                        else
                        {
                            GetComponentInChildren<UbhShotCtrl>().StartShotRoutine();
                            readyToMove = false;
                            readyToFire = false;
                        }
                    }
                    break;
            }
        }
        else
        {
            waitWhileShootTimer -= Time.deltaTime;
            fireTimer -= Time.deltaTime;
            if (waitWhileShootTimer <= 0)
            {
                readyToMove = true;
                waitWhileShootTimer = waitWhileShootTimerReset;
            }
        }
    }
}
