using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadialFire : Enemy
{
    private bool readyToFire = false;
    public float fireTimer = 6f;
    public float fireTimerReset = 6f;
    // Update is called once per frame
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
                        readyToFire = false;
                    }
                }
                break;
        }
    }
}
